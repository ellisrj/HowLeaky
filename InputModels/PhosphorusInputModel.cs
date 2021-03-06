﻿
using System.Xml.Serialization;
using HowLeaky.XmlObjects;
using HowLeaky.CustomAttributes;

namespace HowLeaky.DataModels
{

    //Class for deserialiseing Xml
    public class PEnrichmentOption : IndexData
    {
        [Input("EnrichmentRatio")]
        public double EnrichmentRatio { get; set; }

        [Input("ClayPercentage", "%")]
        public double ClayPercentage { get; set; }                                              // The percent clay in the soil (clay particles are less than 2um in size).

        public PEnrichmentOption() { }
    }

    [XmlRoot("PhosphorusType")]
    public class PhosphorusInputModel : InputModel
    {
        //Input Parameters
        //Xml Elements
        [Input("TotalPConc", "mg/kg")]
        public double TotalPConc { get; set; }                                                  // The total P content of the soil (extracted with hot acid)

        [Input("ColwellP", "mg/kg")]
        public double ColwellP { get; set; }                                                    // The amount of easily-extracted P in the topsoil (0-10 cm, extracted with bicarbonate).

        [Input("")]
        public double PBI { get; set; }                                                         // The degree to which soils bind P (related to the %clay, clay weathering and Fe content)

        [Input("PEnrichmentOption")]
        public PEnrichmentOption PEnrichmentOption { get; set; }                                // The choices are between a constant Enrichment Ration, and a simple function based on Clay percentage (good where variations occur in clay%)

        [Input("DissolvedPOption")]
        public IndexData DissolvedPOption { get; set; }                                         // Two options.  VIC DPI Method: p_max_sorption = 1447 * (1-exp(-0.001 * PBI)), QLD REEF Method: p_max_sorption=5.84*PBI-0.0096*PBI^2  (min of 50). Phos_Conc_Dissolve_mg_per_L is also calculated slightly differently.

        //Getters
        [XmlIgnore]
        public int PEnrichmentOpt { get { return PEnrichmentOption.index; } }
        [XmlIgnore]
        public int DissolvedPOpt { get { return DissolvedPOption.index; } }                     // Two options.  VIC DPI Method: p_max_sorption = 1447 * (1-exp(-0.001 * PBI)), QLD REEF Method: p_max_sorption=5.84*PBI-0.0096*PBI^2  (min of 50). Phos_Conc_Dissolve_mg_per_L is also calculated slightly differently.
        [XmlIgnore]
        public double EnrichmentRatio { get { return PEnrichmentOption.EnrichmentRatio; } }     // The ratio of total P in sediment to the topsoil (0-10 cm).
    }
}
