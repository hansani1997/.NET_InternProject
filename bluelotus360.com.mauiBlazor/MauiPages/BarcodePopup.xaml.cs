namespace bluelotus360.com.mauiBlazor.MauiPages;

public partial class BarcodePopup
{
	public BarcodePopup()
	{
		InitializeComponent();
	}
    private void scanner_BarcodesDetected(Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        scanner.IsDetecting = false;
        Close(e.Results[0].Value);
    }
}