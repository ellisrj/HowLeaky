using System;

namespace HowLeaky.CustomAttributes
{

    public enum AggregationTypeEnum { Mean, Sum, Current,  Max , InCropMean, InCropSum, InCropCurrent}
    public enum AggregationSequenceEnum { InCrop, Fallow, Always }


    public class Output : Attribute
    {
        public String Description { get; set; }
        public String Unit { get; set;}
        public double Scale { get; set; } = 1;

        public AggregationTypeEnum AggregationType { get; set; } = AggregationTypeEnum.Mean;
        public AggregationSequenceEnum AggregationSequence { get; set; } = AggregationSequenceEnum.Always;

        public int TimeSeriesOutputIdentifier { get; set; }

        public Output() { }

        public Output(string Description ="", string Unit = "", double Scale = 1, AggregationTypeEnum AggregationType = AggregationTypeEnum.Mean, AggregationSequenceEnum AggregationSequence = AggregationSequenceEnum.Always, int theTimeSeriesID = -1)
        {
            this.Description = Description;
            this.Unit = Unit;
            this.Scale = Scale;

            this.AggregationType = AggregationType;
            this.AggregationSequence = AggregationSequence;

            this.TimeSeriesOutputIdentifier = theTimeSeriesID;
        }

        public Output(string Description = "", string Unit = "", double Scale = 1, AggregationTypeEnum AggregationType = AggregationTypeEnum.Mean)
        {
            this.Description = Description;
            this.Unit = Unit;
            this.Scale = Scale;

            this.AggregationType = AggregationType;

        }

        public Output(string Description = "", string Unit = "", double Scale = 1, AggregationTypeEnum AggregationType = AggregationTypeEnum.Mean, int theTimeSeriesID = -1)
        {
            this.Description = Description;
            this.Unit = Unit;
            this.Scale = Scale;

            this.AggregationType = AggregationType;

            this.TimeSeriesOutputIdentifier = theTimeSeriesID;
        }

        public Output(string Description = "", string Unit = "", double Scale = 1, int theTimeSeriesID = -1)
        {
            this.Description = Description;
            this.Unit = Unit;
            this.Scale = Scale;

            this.TimeSeriesOutputIdentifier = theTimeSeriesID;
        }

        public Output(string Description = "", string Unit = "", int theTimeSeriesID = -1)
        {
            this.Description = Description;
            this.Unit = Unit;

            this.TimeSeriesOutputIdentifier = theTimeSeriesID;
        }

        public Output(string Description = "", string Unit = "")
        {
            this.Description = Description;
            this.Unit = Unit;
        }

        public Output(string Description = "", int theTimeSeriesID = -1)
        {
            this.Description = Description;

            this.TimeSeriesOutputIdentifier = theTimeSeriesID;
        }

        public Output(string Description = "")
        {
            this.Description = Description;

        }
    }
}
