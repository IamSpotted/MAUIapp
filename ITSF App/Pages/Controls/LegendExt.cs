using Syncfusion.Maui.Toolkit.Charts;

namespace ITSF_App.Pages.Controls
{
    // Custom Legend class extending the Syncfusion ChartLegend
    public class LegendExt : ChartLegend
    {
        // This method is overridden to provide a custom size coefficient for the legend
        protected override double GetMaximumSizeCoefficient()
        {
            // Returning a value of 0.5 for maximum size, limiting how large the legend can be
            return 0.5;
        }
    }
}