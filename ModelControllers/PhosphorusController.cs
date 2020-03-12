﻿using HowLeaky.CustomAttributes;
using HowLeaky.Tools.Helpers;
using HowLeaky.DataModels;
using System;
using HowLeaky.OutputModels;
using System.Collections.Generic;
using HowLeaky.Interfaces;

namespace HowLeaky.ModelControllers
{
    //public class PhosphorusOutputDataModel : OutputDataModel, IDailyOutput
    //{
        
    //}

    public class PhosphorusController : HLController
    {
        //TODO: Change to enums
        static int ENRICHMENT_RATIO = 0;
        static int ENRICHMENT_CLAY = 1;
        static int DISSOLVED_P_VICDPI = 0;
        static int DISSOLVED_P_QLDREEF = 1;

        //public	double so_DissolvePExport_kg_per_ha{get;set;}		// Dissolved P export(kg/ha)
        //public	double so_EMC_mg_per_l{get;set;}						// Phosphorus EMC(mg/L)

        public double MaxPhosConcBioParticmgPerL { get; set; }
        public double MaxPhosConcBiomgPerL { get; set; }
        public double MaxPhosConcParticmgPerL { get; set; }
        public double MaxPhosConcTotalmgPerL { get; set; }
        public double MaxPhosConcDissolvemgPerL { get; set; }

        public PhosphorusInputModel InputModel { get; set; }
        //public PhosphorusOutputDataModel Output { get; set; }

        //Reportable Outputs
        [Output("Particulate P Conc", "mg/l", 6001)]
        public double ParticulateConc { get; set; }        
        [Output("Dissolved P Conc", "mg/l", 6002)]
        public double DissolvedConc { get; set; }          
        [Output("Bioavailable particulate P Conc", "mg/l", 6003)]
        public double BioAvailParticPConc { get; set; }    
        [Output("Bioavailable P Conc", "mg/l", 6004)]
        public double BioAvailPConc { get; set; }          
        [Output("Total P Conc", "mg/l", 6005)]
        public double TotalPConc { get; set; }             
        [Output("Particulate P export", "kg/ha", 6006)]
        public double ParticPExport { get; set; }          
        [Output("Bioavailable particulate P export", "kg/ha", 6008)]
        public double BioAvailParticPExport { get; set; } 
        [Output("Bioavailable P export", "kg/ha", 6009)]
        public double TotalBioAvailExport { get; set; }   
        [Output("Total Phosphorus export", "kg/ha", 6010)]
        public double TotalP { get; set; }
        //[Output("CKQ", "t/ha", 6011)]
        //public double CKQ { get; set; }
        [Output("PPHLC", "kg/ha", 6012)]
        public double PPHLC { get; set; }
        [Output("Dissolved P Export","kg/ha" , 6007)]
        public double PhosExportDissolve { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PhosphorusController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public PhosphorusController(Simulation sim) : base(sim)
        {
            //Output = new PhosphorusOutputDataModel();
        }
        /// <summary>
        /// 
        /// </summary>
        public PhosphorusController(Simulation sim, List<InputModel> inputModels) : this(sim)
        {
            InputModel = (PhosphorusInputModel)inputModels[0];

            InitOutputModel();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
            //Do nothing
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitialisePhosphorusParameters()
        {
            MaxPhosConcBioParticmgPerL = 0;
            MaxPhosConcBiomgPerL = 0;
            MaxPhosConcParticmgPerL = 0;
            MaxPhosConcTotalmgPerL = 0;
            MaxPhosConcDissolvemgPerL = 0;

            ParticulateConc = 0;
            DissolvedConc = 0;
            BioAvailParticPConc = 0;
            BioAvailPConc = 0;
            TotalPConc = 0;
            ParticPExport = 0;
            BioAvailParticPExport = 0;
            TotalBioAvailExport = 0;
            TotalP = 0;
            PPHLC = 0;
            PhosExportDissolve = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdatePhosphorusSummaryValues() { }
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                if (CansimulatePhosphorus())
                {
                    if (Sim.SoilController.Runoff > 0)
                    {
                        CalculateDissolvedPhosphorus();
                        CalculateParticulatePhosphorus();
                        CalculateTotalPhosphorus();
                        CalculateBioavailableParticulatePhosphorus();
                        CalculateBioavailablePhosphorus();
                        TestMaximumPhosphorusConcentrations();
                    }
                    else
                    {
                        ResetPhosphorusOutputParameters();

                    }
                    CalculateCATCHMODSOutputs();
                }
                UpdatePhosphorusSummaryValues();
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override InputModel GetInputModel()
        {
            return InputModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CansimulatePhosphorus()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDissolvedPhosphorus()
        {

            double phosSaturationIndex = 0;
            double pMaxSorption = 0;
            if (InputModel.DissolvedPOpt == DISSOLVED_P_VICDPI)
            {
                pMaxSorption = 1447 * (1 - Math.Exp(-0.001 * InputModel.PBI));
            }
            else
            {
                pMaxSorption = 5.84 * InputModel.PBI - 0.0096 * Math.Pow(InputModel.PBI, 2);
                if (pMaxSorption < 50)
                {
                    pMaxSorption = 50;
                }
            }

            double p_enrich = CalculatePhosphorusEnrichmentRatio();
            if (!MathTools.DoublesAreEqual(pMaxSorption, 0))
            {
                phosSaturationIndex = (InputModel.ColwellP * p_enrich) / (pMaxSorption) * 100.0;
            }
            else
            {
                phosSaturationIndex = 0;
                MathTools.LogDivideByZeroError("CalculateDissolvedPhosphorus", "p_max_sorption", "phos_saturation_index");
            }

            if (InputModel.DissolvedPOpt == DISSOLVED_P_VICDPI)
            {
                //The following is the original fn published at MODSIM11:
                if (phosSaturationIndex < 5)
                {
                    DissolvedConc = 10.0 * phosSaturationIndex / 1000.0;
                }
                else
                {
                    DissolvedConc = (-100.0 + 30 * phosSaturationIndex) / 1000.0;
                }
            }
            else
            {
                if (phosSaturationIndex < 10)
                {
                    DissolvedConc = 7.5 * phosSaturationIndex / 1000.0;
                }
                else
                {
                    DissolvedConc = ((-200.0 + 27.5 * phosSaturationIndex) / 1000.0);
                }
            }
            PhosExportDissolve = (DissolvedConc / 1000000.0 * Sim.SoilController.Runoff * 10000.0);//CHECK - this wasn't marked as an output parameter
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateParticulatePhosphorus()
        {
            double pEnrich = CalculatePhosphorusEnrichmentRatio();
            double pSedConcGPerL = 0;
            if (!MathTools.DoublesAreEqual(Sim.SoilController.Runoff, 0))//&&sim.in_SedDelivRatio!=0)
            {
                // convert t/ha to g/ha and sim.out_WatBal_Runoff_mm(mm) to L/ha.  Then the division yields g/L of sediment.
                pSedConcGPerL = Sim.SoilController.HillSlopeErosion * 1000000.0 / (Sim.SoilController.Runoff * 10000.0) * Sim.SoilController.InputModel.SedDelivRatio;
            }
            // convert sed conc from g/L to mg/L and totalPconc from mg/kg (I assume it is in mg/kg) to g/g
            ParticulateConc = (pSedConcGPerL * 1000.0 * InputModel.TotalPConc / 1000000.0 * pEnrich);
            ParticPExport = (ParticulateConc / 1000000.0 * Sim.SoilController.Runoff * 10000.0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalculatePhosphorusEnrichmentRatio()
        {
            if (InputModel.PEnrichmentOpt == ENRICHMENT_RATIO)
            {
                //input a constant (range = 1 to ~10).
                return InputModel.EnrichmentRatio;
            }
            else if (InputModel.PEnrichmentOpt == ENRICHMENT_CLAY)
            {
                //Clay%.  This function is based on a few data from Qld field experiments.
                //Penrichment=MIN(10,MAX(1,15-0.33*clay)). The results of this funtion are:
                return Math.Min(10.0, Math.Max(1, 15 - 0.33 * InputModel.PEnrichmentOption.ClayPercentage));
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateTotalPhosphorus()
        {
            TotalPConc = (DissolvedConc + ParticulateConc);
            TotalP = (PhosExportDissolve + ParticPExport); //CHECK Phos_Export_Dissolve wasn't marked as out;

        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateBioavailableParticulatePhosphorus()
        {
            double pA;
            if (!MathTools.DoublesAreEqual(InputModel.TotalPConc, 0))
            {
                pA = InputModel.ColwellP * 1.2 / InputModel.TotalPConc;
            }
            else
            {
                pA = 0;
                //	LogDivideByZeroError("CalculateBioavailableParticulatePhosphorus","in_TotalPConc_mg_per_kg","pA");
            }
            BioAvailParticPConc = (ParticulateConc * pA);
            BioAvailParticPExport = (ParticPExport * pA);
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateBioavailablePhosphorus()
        {
            BioAvailPConc = (0.8 * DissolvedConc + BioAvailParticPConc);
            TotalBioAvailExport = (0.8 * PhosExportDissolve + BioAvailParticPExport);
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateCATCHMODSOutputs()
        {
            if (Sim.SoilController.Runoff > 0 && Sim.SoilController.HillSlopeErosion > 0)
            {
                if (!MathTools.DoublesAreEqual(Sim.SoilController.InputModel.SedDelivRatio, 0) && !MathTools.DoublesAreEqual(Sim.SoilController.UsleLsFactor, 0))
                {
                    PPHLC = (ParticPExport / (Sim.SoilController.InputModel.SedDelivRatio * Sim.SoilController.UsleLsFactor));
                }
                else
                {
                    PPHLC = 0;
                }
            }
            else
            {
                PPHLC = 0;
            }
            //CKQ = (Sim.SoilController.SedCatchmod);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetPhosphorusOutputParameters()
        {
            MaxPhosConcBioParticmgPerL = 0;
            MaxPhosConcBiomgPerL = 0;
            MaxPhosConcParticmgPerL = 0;
            MaxPhosConcTotalmgPerL = 0;
            MaxPhosConcDissolvemgPerL = 0;

            ParticulateConc = 0;
            DissolvedConc = 0;
            BioAvailParticPConc = 0;
            BioAvailPConc = 0;
            TotalPConc = 0;
            ParticPExport = 0;
            BioAvailParticPExport = 0;
            TotalBioAvailExport = 0;
            TotalP = 0;
            PPHLC = 0;
            PhosExportDissolve = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void TestMaximumPhosphorusConcentrations()
        {
            if (MaxPhosConcParticmgPerL < ParticulateConc)
            {
                MaxPhosConcParticmgPerL = ParticulateConc;
            }
            if (MaxPhosConcTotalmgPerL < TotalPConc)
            {
                MaxPhosConcTotalmgPerL = TotalPConc;
            }
            if (MaxPhosConcDissolvemgPerL < DissolvedConc)
            {
                MaxPhosConcDissolvemgPerL = DissolvedConc;
            }
            if (MaxPhosConcBioParticmgPerL < BioAvailParticPConc)
            {
                MaxPhosConcBioParticmgPerL = BioAvailParticPConc;
            }
            if (MaxPhosConcBiomgPerL < TotalPConc)
            {
                MaxPhosConcBiomgPerL = BioAvailPConc;
            }
        }
    }
}
