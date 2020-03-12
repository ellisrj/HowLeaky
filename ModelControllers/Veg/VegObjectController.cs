﻿using HowLeaky.CustomAttributes;
using HowLeaky.Interfaces;
using HowLeaky.OutputModels;
using HowLeaky.Tools.Helpers;
using System;
using System.Xml.Serialization;

namespace HowLeaky.ModelControllers.Veg
{
    public class VegObjectSummaryOutputModel : OutputDataModel
    {
        [Unit("mm")]
        public double AvgCropRainfall { get; set; }
        [Unit("mm")]
        public double AvgCropIrrigation { get; set; }
        [Unit("mm")]
        public double AvgCropRunoff { get; set; }
        [Unit("mm")]
        public double CropSoilEvaporation { get; set; }
        [Unit("mm")]
        public double CropTranspiration { get; set; }
        [Unit("mm")]
        public double AvgCropEvapoTranspiration { get; set; }
        [Unit("mm")]
        public double AvgCropOverflow { get; set; }
        [Unit("mm")]
        public double AvgCropDrainage { get; set; }
        [Unit("mm")]
        public double AvgCropLateralFlow { get; set; }
        [Unit("t_per_ha")]
        public double AvgCropSoilErrosion { get; set; }
        [Unit("t_per_ha")]
        public double AnnualCropSedimentDelivery { get; set; }
    }

    public class VegObjectAggregateOutputModel : OutputDataModel
    {
        public double CropRainfall { get; set; }
        public double CropIrrigation { get; set; }
        public double CropRunoff { get; set; }
        public double CropSoilEvaporation { get; set; }
        public double CropTranspiration { get; set; }
        public double CropEvapotranspiration { get; set; }
        public double CropOverflow { get; set; }
        public double CropDrainage { get; set; }
        public double CropLateralFlow { get; set; }
        public double CropSoilErosion { get; set; }
    }

    public class VegObjectController : HLController, IChildController
    {
        public bool TodayIsHarvestDay { get; set; }
        public bool PredefinedResidue { get; set; }
        public CropStatus CropStatus { get; set; }
        public DateTime LastSowingDate { get; set; } = DateUtilities.NULLDATE;
        public DateTime LastHarvestDate { get; set; } = DateUtilities.NULLDATE;
        public DateTime FirstRotationDate { get; set; } = DateUtilities.NULLDATE;
        //public int DaysSincePlanting { get; set; }
        public int RotationCount { get; set; }
        public int MissedRotationCount { get; set; }
        //public int PlantingCount { get; set; }
        //public int HarvestCount { get; set; }
        //public int CropDeaths { get; set; }
        //public int NumberOfFallows { get; set; }
        public int KillDays { get; set; }
        public double MaximumRootDepth { get; set; }
        public double TotalTranspiration { get; set; }

        //These should be reported
        [Output("Crop Stage", "", 1, AggregationTypeEnum.Current, AggregationSequenceEnum.InCrop, 7001)]
        public double CropStage { get; set; }
        public double CropCover { get; set; }
        public double CropCoverPercent { get; set; }
        public double CropResidue { get; set; }
        //public double Runoff { get; set; }
        //public double Drainage { get; set; }
        public double TotalEvapotranspiration { get; set; }
        public double SoilWaterAtPlanting { get; set; }
        public double SoilWaterAtHarvest { get; set; }
        public double CumulativeYield { get; set; }
        public double AccumulatedCover { get; set; }
        public double AccumulatedResidue { get; set; }
        public double AccumulatedTranspiration { get; set; }

        //Reportable Outputs
        [Output(" Days since planting", "days", 1, AggregationTypeEnum.Current, AggregationSequenceEnum.InCrop, 4001)]
        public int DaysSincePlanting { get; set; }
        [Output("Leaf area index", "LAI", 1, AggregationTypeEnum.Current, AggregationSequenceEnum.InCrop, 4002)]
        public double LAI { get; set; }
        [Output("Living vegetation cover expressed as a percentage of total area", "%", 100, 4003)]
        public double GreenCover { get; set; }
        [Output("Dead vegetation cover expressed as a percentage of total area", "%", 100, 4006)]
        public double ResidueCover { get; set; }
        [Output("Living and dead cover, calculated using Beer's Law, and expressed as a percentage of total area", "%", 100, 4004)]
        public double TotalCover { get; set; }
        [Output("Residue biomass amount", "kg/ha", 4005)]
        public double ResidueAmount { get; set; }
        [Output("Cumulative dry matter", "kg/ha", 1, AggregationTypeEnum.Current, AggregationSequenceEnum.InCrop, 4007)]
        public double DryMatter { get; set; }
        [Output("Root depth", "mm", 1, AggregationTypeEnum.Current, AggregationSequenceEnum.InCrop, 4008)]
        public double RootDepth { get; set; }
        [Output("Yield", "t/h", 1, AggregationTypeEnum.Current, AggregationSequenceEnum.InCrop, 4009)]
        public double Yield { get; set; }
        [Output("Potential transpiration", "mm", 1, AggregationTypeEnum.Sum, AggregationSequenceEnum.InCrop, 4010)]
        public double PotTranspiration { get; set; }
        [Output("Growth regulator", 4011)]
        public double GrowthRegulator { get; set; }
        [Output("Water stress index", 7004)]
        public double WaterStressIndex { get; set; }
        [Output("Temperature stress index", 7005)]
        public double TempStressIndex { get; set; }
        [Output("Crop Rainfall", "mm", 1, AggregationTypeEnum.Sum)]
        public double CropRainfall { get; set; }
        [Output("Crop Irrigation", "mm", 1, AggregationTypeEnum.Sum)]
        public double CropIrrigation { get; set; }
        [Output("Crop Runoff", "mm", 1, AggregationTypeEnum.Sum)]
        public double CropRunoff { get; set; }
        [Output("Crop Soil Evaporation", "mm", 1, AggregationTypeEnum.Sum)]
        public double SoilEvaporation { get; set; }
        [Output("Crop Transpiration", "mm", 1, AggregationTypeEnum.Sum)]
        public double CropTranspiration { get; set; }
        [Output(" Crop Evapotranspiration", "mm", 1, AggregationTypeEnum.Sum)]
        public double CropEvapoTranspiration { get; set; } //Not currently set
        [Output("Crop Drainage", "mm", 1, AggregationTypeEnum.Sum)]
        public double CropDrainage { get; set; }
        [Output("Crop Lateral Flow", "mm", 1, AggregationTypeEnum.Sum)]
        public double CropLateralFlow { get; set; }
        [Output("Crop Overflow", "mm", 1, AggregationTypeEnum.Sum)]
        public double CropOverflow { get; set; }
        [Output("Crop Soil Erosion", "t/h", 1, AggregationTypeEnum.Sum)]
        public double CropSoilErosion { get; set; }
        [Output("Crop Off Site Sediment Delivery", "t/h", 1, AggregationTypeEnum.Sum)]
        public double CropSedimentDelivery { get; set; }
        [Output("Number of crops Planted")]
        public double PlantingCount { get; set; }
        [Output("Number of crops Harvested")]
        public double HarvestCount { get; set; }
        [Output("Number of crops Killed")]
        public double CropDeathCount { get; set; }
        [Output("Number of fallows")]
        public double FallowCount { get; set; }

        //Summary variables
        [Output("Avg. Yield per Harvest", "kg/ha/harvest")]
        public double YieldPerHarvest { get; set; }
        [Output("Avg. Yield per Planting", "kg/ha/plant")]
        public double YieldPerPlant { get; set; }
        [Output("Avg. Yield per Year", "kg/ha/yr")]
        public double YieldPerYear { get; set; }
        [Output("Yield/Transpiration", "kg/ha/mm")]
        public double YieldDivTranspir { get; set; }
        [Output("Residue Cover/Transpiration", "%/mm")]
        public double ResidueCovDivTranspir { get; set; }
        [Output("Crops Harvested / Crops Planted")]
        public double CropsHarvestedDivCropsPlanted { get; set; }
        //[Output("Potential Maximum LAI")]
        //public double PotMaxLAI { get; set; } // Not wired up to anything

        //public VegObjectOutputModel Output { get; set; } = new VegObjectOutputModel();
        public VegObjectSummaryOutputModel SO;
        public VegObjectAggregateOutputModel Sum;

        /// <summary>
        /// 
        /// </summary>
        public bool TodayIsPlantDay
        {
            get
            {
                return DaysSincePlanting == 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CropIndex
        {
            get
            {
                return Sim.VegetationController.GetCropIndex(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public VegetationController VegetationController { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetExistsInTheGround()
        {
            return (CropStatus == CropStatus.Planting || CropStatus == CropStatus.Growing);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ExistsInTheGround
        {
            get
            {
                return GetExistsInTheGround();
            }
        }

        //Virtual methods

        public virtual void ResetCropParametersAfterHarvest() { }
        public virtual bool IsCropUnderMaxContinuousRotations() { return true; }
        public virtual bool SatisifiesMultiPlantInWindow() { return true; }
        public virtual bool HasCropHadSufficientContinuousRotations() { return true; }
        public virtual bool HasCropBeenAbsentForSufficientYears(DateTime today) { return true; }
        public virtual bool IsReadyToPlant() { return true; }
        public virtual bool StillRequiresIrrigating() { return false; }
        public virtual bool IsSequenceCorrect() { return true; }
        public virtual bool IsGrowing() { return (CropStatus == CropStatus.Growing); }
        public virtual bool InitialisedMeasuredInputs() { return false; }
        public virtual bool DoesCropMeetSowingCriteria() { return false; }
        public virtual bool GetIsLAIModel() { return false; }
        public virtual bool GetInFallow() { return (CropStatus == CropStatus.Fallow); }
        public virtual double GetPotentialSoilEvaporation() { return 0; }
        public virtual double GetTotalCover() { return 0; }
        public virtual double CalculatePotentialTranspiration() { return 0; }


        /// <summary>
        /// 
        /// </summary>
        public VegObjectController() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public VegObjectController(Simulation sim) : base(sim)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ResetRotationCount()
        {
            RotationCount = 0;
            MissedRotationCount = 0;
            FirstRotationDate = Sim.Today;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetIsPlanting()
        {
            //TOD:Check this ishould be double
            return (MathTools.DoublesAreEqual(DaysSincePlanting, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateTranspiration()
        {
            for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
            {
                Sim.SoilController.LayerTranspiration[i] = 0;
            }
            PotTranspiration = CalculatePotentialTranspiration();

            if (PotTranspiration > 0)
            {
                double psup;

                double[] density = new double[10];
                double[] supply = new double[10];
                double[] rootPenetration = new double[10];


                // initialize transpiration array
                //Didn't we just do this !
                //for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                //{
                //    Sim.SoilController.LayerTranspiration[i] = 0;
                //}
                //  Calculate soil water supply index

                for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                {
                    double denom = Sim.SoilController.DrainUpperLimitRelWP[i];
                    if (!MathTools.DoublesAreEqual(denom, 0))
                    {
                        Sim.SoilController.MCFC[i] = MathTools.CheckConstraints(Sim.SoilController.SoilWaterRelWP[i] / denom, 1.0, 0.0);
                    }
                    else
                    {
                        Sim.SoilController.MCFC[i] = 0;
                        //LogDivideByZeroError("CalculateTranspiration","sim.DrainUpperLimit_rel_wp[i]","sim.mcfc[i]");
                    }

                    //TODO: the 0.3 here is the SWPropForNoStress variable - need to implement

                    if (Sim.SoilController.MCFC[i] >= 0.3)
                    {
                        supply[i] = 1.0;
                    }
                    else
                    {
                        supply[i] = Sim.SoilController.MCFC[i] / 0.3;
                    }
                }

                //  Calculate root penetration per layer (root_penetration)
                //  Calculate root density per layer (density)

                rootPenetration[0] = 1.0;
                density[0] = 1.0;
                for (int i = 1; i < Sim.SoilController.LayerCount; ++i)
                {
                    if (Sim.SoilController.Depth[i + 1] - Sim.SoilController.Depth[i] > 0)
                    {
                        rootPenetration[i] = Math.Min(1.0, Math.Max(RootDepth - Sim.SoilController.Depth[i], 0) / (Sim.SoilController.Depth[i + 1] - Sim.SoilController.Depth[i]));
                        if (Sim.SoilController.Depth[i + 1] > 300)
                        {
                            if (!MathTools.DoublesAreEqual(MaximumRootDepth, 300))
                            {
                                //density[i] = Math.Max(0.0, (1.0 - 0.50 * Math.Min(1.0, (Sim.SoilController.Depth[i + 1] - 300.0) / (MaximumRootDepth - 300.0))));
                                density[i] = Math.Max(0.0, (1.0 - 0.50 * Math.Min(1.0, (Sim.SoilController.Depth[i + 1] - 300.0) / (RootDepth - 300.0))));

                            }
                            else
                            {
                                density[i] = 0.5;
                                //dont log this error
                            }
                        }
                        else
                        {
                            density[i] = 1.0;
                        }
                    }
                    else
                    {
                        rootPenetration[i] = 0;
                        density[i] = 1.0;
                        //string text1="sim.depth["+string(i+1)+"]-sim.depth["+string(i)+"] ("+FormatFloat("0.#",sim.depth[i+1])+"-"+FormatFloat("0.#",sim.depth[i])+")";
                        //LogDivideByZeroError("CalculateTranspiration",text1,"root_penetration["+String(i)+"]");
                    }
                }

                // Calculate transpiration from each layer

                psup = 0;
                for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                {
                    if (rootPenetration[i] < 1.0 && Sim.SoilController.MCFC[i] <= (1.0 - rootPenetration[i]))
                    {
                        Sim.SoilController.LayerTranspiration[i] = 0.0;
                    }
                    else
                    {
                        Sim.SoilController.LayerTranspiration[i] = density[i] * supply[i] * PotTranspiration;
                    }
                    if (Sim.SoilController.LayerTranspiration[i] > Sim.SoilController.SoilWaterRelWP[i])
                    {
                        Sim.SoilController.LayerTranspiration[i] = Math.Max(0.0, Sim.SoilController.SoilWaterRelWP[i]);
                    }
                    psup = psup + Sim.SoilController.LayerTranspiration[i];
                }

                // reduce transpiration if more than potential transpiration
                if (!MathTools.DoublesAreEqual(psup, 0) && psup > PotTranspiration)
                {
                    for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                    {
                        Sim.SoilController.LayerTranspiration[i] *= PotTranspiration / psup;
                    }
                }
                TotalTranspiration = 0;
                for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                {
                    TotalTranspiration += Sim.SoilController.LayerTranspiration[i];
                }
            }
            else
            {
                for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                {
                    Sim.SoilController.LayerTranspiration[i] = 0;
                }
                TotalTranspiration = 0;
            }
            AccumulatedTranspiration += TotalTranspiration;

            CropTranspiration = TotalTranspiration;

            CropEvapoTranspiration = CropTranspiration + Sim.SoilController.SoilEvap;

            Sim.SoilController.EvapoTransp += CropTranspiration;
            Sim.SoilController.Transpiration = CropTranspiration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalcFallowSoilWater()
        {
            if (SoilWaterAtHarvest > 0)
            {
                return SoilWaterAtPlanting - SoilWaterAtHarvest;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Plant()
        {
            ++Sim.SoilController.TotalNumberPlantings;
            Sim.SoilController.AccumulateCovDayBeforePlanting += Sim.SoilController.TotalResidueCover;
            VegObjectController lastcrop = Sim.VegetationController.CurrentCrop;
            Sim.VegetationController.CurrentCrop = this;
            LastSowingDate = Sim.Today;
            if (lastcrop != this || RotationCount == 0)
            {
                lastcrop.ResetRotationCount();
                Sim.VegetationController.CurrentCrop.ResetRotationCount();
            }

            DaysSincePlanting = 0;
            ++PlantingCount;
            DryMatter = 0;
            DryMatter = 0;
            CropStage = 0;
            RootDepth = 0;
            Yield = 0;
            SoilWaterAtPlanting = Sim.SoilController.TotalSoilWater;
            CropStatus = CropStatus.Growing;

        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetCover()
        {
            GreenCover = 0;
            CropCover = 0;
            CropCoverPercent = 0;
            TotalCover = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateResidue()
        {
            AccumulatedResidue += ResidueCover * 100.0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitialiseCropSummaryParameters()
        {
            YieldPerHarvest = 0;
            YieldPerPlant = 0;
            YieldPerYear = 0;
            CropsHarvestedDivCropsPlanted = 0;
            YieldDivTranspir = 0;
            ResidueCovDivTranspir = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCropIndex()
        {
            return Sim.VegetationController.GetCropIndex(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateCropSummary()
        {
            double numyears = (double)(Sim.NumberOfDaysInSimulation / 365.0);

            //TODO: Should this be a double
            if (!MathTools.DoublesAreEqual(HarvestCount, 0))
            {
                YieldPerHarvest = CumulativeYield / (double)HarvestCount;
            }
            else
            {
                YieldPerHarvest = 0;
            }
            YieldPerYear = CumulativeYield / numyears;

            if (!MathTools.DoublesAreEqual(PlantingCount, 0))
            {
                CropsHarvestedDivCropsPlanted = HarvestCount / (double)PlantingCount * 100.0;
                YieldPerPlant = CumulativeYield / (double)PlantingCount;
            }
            else
            {
                CropsHarvestedDivCropsPlanted = 0;
                YieldPerPlant = 0;
            }
            if (!MathTools.DoublesAreEqual(AccumulatedTranspiration, 0))
            {
                YieldDivTranspir = CumulativeYield / AccumulatedTranspiration;
                ResidueCovDivTranspir = AccumulatedResidue / AccumulatedTranspiration;
            }
            else
            {
                YieldDivTranspir = 0;
                ResidueCovDivTranspir = 0;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateCropWaterBalance()
        {
            try
            {
                if (ExistsInTheGround)
                {
                    Sum.CropRainfall += Sim.ClimateController.Rain;
                    Sum.CropIrrigation += Sim.SoilController.Irrigation;
                    Sum.CropRunoff += Sim.SoilController.Runoff;
                    Sum.CropSoilEvaporation += Sim.SoilController.SoilEvap;
                    Sum.CropTranspiration += TotalTranspiration;
                    Sum.CropEvapotranspiration = Sum.CropSoilEvaporation + Sum.CropTranspiration;
                    Sum.CropOverflow += Sim.SoilController.Overflow;
                    Sum.CropDrainage += Sim.SoilController.DeepDrainage;
                    Sum.CropLateralFlow += Sim.SoilController.LateralFlow;
                    Sum.CropSoilErosion += Sim.SoilController.HillSlopeErosion;
                }

            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateMonthlyOutputs()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateSummaryOutputs()
        {
            double denom = PlantingCount;
            SO.AvgCropRainfall = MathTools.Divide(Sum.CropRainfall, denom);
            SO.AvgCropIrrigation = MathTools.Divide(Sum.CropIrrigation, denom);
            SO.AvgCropRunoff = MathTools.Divide(Sum.CropRunoff, denom);
            SO.CropSoilEvaporation = MathTools.Divide(Sum.CropSoilEvaporation, denom);
            SO.CropTranspiration = MathTools.Divide(Sum.CropTranspiration, denom);
            SO.AvgCropEvapoTranspiration = MathTools.Divide(Sum.CropEvapotranspiration, denom);
            SO.AvgCropOverflow = MathTools.Divide(Sum.CropOverflow, denom);
            SO.AvgCropDrainage = MathTools.Divide(Sum.CropDrainage, denom);
            SO.AvgCropLateralFlow = MathTools.Divide(Sum.CropLateralFlow, denom);
            SO.AvgCropSoilErrosion = MathTools.Divide(Sum.CropSoilErosion, denom);
            SO.AnnualCropSedimentDelivery = MathTools.Divide(Sum.CropSoilErosion, denom) * Sim.SoilController.InputModel.SedDelivRatio;
        }
    }
}
