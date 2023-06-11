namespace bluelotus360.com.mauiBlazor.MauiPages;

public partial class YesNoPopup
{
	public YesNoPopup(string myTxt)
	{
		InitializeComponent();
		this.myText.Text = myTxt;
	}

    void OnYesButtonClicked(object? sender, EventArgs e) => Close(true);

    void OnNoButtonClicked(object? sender, EventArgs e) => Close(false);
}