using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class Program
{
	public static void Main()
	{
		var test = new PositionsCtrlTest();
		test.Should_Calculate_Daily_Pnls();
		test.Should_Set_Cash_Delta_To_Zero();
	}
}

public class PositionsController
{
	public IEnumerable<Position> Positions
	{
		get;
		set;
	}

	public IEnumerable<Position> GetPositions()
{
    // Use the Zip method to combine each position with the previous position
    var positionsWithPrev = Positions.Zip(Positions.Skip(1), (curr, prev) => new { curr, prev });

    // Use LINQ's Select method to create a new list of positions with the calculated daily PnL values
    var positions = positionsWithPrev.Select(x =>
    {
        var newPos = new Position
        {
            // Copy the values of the current position
            PositionDate = x.curr.PositionDate,
            InstrumentId = x.curr.InstrumentId,
            InstrumentType = x.curr.InstrumentType,
            YtdPnl = x.curr.YtdPnl,
            DeltaExp = x.curr.DeltaExp
        };

        // Calculate the daily PnL by subtracting the YTD PnL of the previous position from the current position's YTD PnL
        newPos.DailyPnl = x.curr.YtdPnl - x.prev.YtdPnl;

        return newPos;
    });

    // Use LINQ's Where method to filter the list of positions to only include those that have an InstrumentType of "Cash"
    var cashPositions = positionsWithPrev.Where(x => x.curr.InstrumentType == "Cash");

    // Use the ForEach method to set the DeltaExp property of each cash position to zero
    cashPositions.ForEach(x => x.curr.DeltaExp = 0m);

    // Return the list of positions with the updated DailyPnl and DeltaExp values
    return positions;
}

}

public class PositionsCtrlTest
{
	public void Should_Calculate_Daily_Pnls()
	{
		var positions = new List<Position>()
		{
			new Position{PositionDate = "2019-10-01", InstrumentId = "Eq A", DailyPnl = null, YtdPnl = 100}, 
			new Position{PositionDate = "2019-10-02", InstrumentId = "Eq A", DailyPnl = null, YtdPnl = 110}, 
			new Position{PositionDate = "2019-10-03", InstrumentId = "Eq A", DailyPnl = null, YtdPnl = 100}, 
		};
		var ctrl = new PositionsController{Positions = positions};
		
		var res = ctrl.GetPositions();
		Assert.Equal(3, res.Count());
		
		var resPositions = res.ToList();
		var oct2Pos = resPositions[1];
		Assert.NotNull(oct2Pos);
		Assert.Equal(10m, oct2Pos.DailyPnl);
		
		var oct3Pos = resPositions[2];
		Assert.NotNull(oct3Pos);
		Assert.Equal(-10m, oct3Pos.DailyPnl);
	}

	public void Should_Set_Cash_Delta_To_Zero()
	{
		var positions = new List<Position>()
		{
			new Position{PositionDate = "2019-10-01", InstrumentId = "Eq A", InstrumentType = "Equity", DeltaExp = 2000m}, 
			new Position{PositionDate = "2018-01-01", InstrumentId = "USD", InstrumentType = "Cash", DeltaExp = 1000m}, 
		};
		var ctrl = new PositionsController{Positions = positions};
		
		var res = ctrl.GetPositions();
		Assert.Equal(2, res.Count());
		
		var cashPositions = res.Where(x => x.InstrumentType == "Cash");
		Assert.True(cashPositions.All(x => x.DeltaExp == 0m));
		
		var eqPositions = res.Where(x => x.InstrumentType == "Equity");
		Assert.True(eqPositions.All(x => x.DeltaExp != 0m));
	}
}

public class Position
{
	public string PositionDate
	{
		get;
		set;
	}

	public string InstrumentId
	{
		get;
		set;
	}

	public string InstrumentType
	{
		get;
		set;
	}

	public decimal? DailyPnl //當日損益
	{
		get;
		set;
	}

	public decimal? YtdPnl
	{
		get;
		set;
	}

	public decimal? DeltaExp
	{
		get;
		set;
	}
}