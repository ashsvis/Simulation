namespace Simulator.View
{
    public static class ThemeManager
    {
        public static void ApplyDarkTheme(Form form)
        {
            form.BackColor = Color.FromArgb(45, 45, 48); // Dark background
            form.ForeColor = Color.FromArgb(220, 220, 220); // Light text


            foreach (Control control in form.Controls)
            {
                ApplyDarkThemeToControl(control);
            }
        }


        private static void ApplyDarkThemeToControl(Control control)
        {
            control.BackColor = Color.FromArgb(60, 63, 65); // Darker background for controls
            control.ForeColor = Color.FromArgb(220, 220, 220); // Light text


            if (control is TextBox || control is RichTextBox)
            {
                control.BackColor = Color.FromArgb(30, 30, 30); // Even darker for text boxes
                control.ForeColor = Color.FromArgb(200, 200, 200);
            }
            else if (control is Button)
            {
                control.BackColor = Color.FromArgb(45, 45, 48);
                control.ForeColor = Color.FromArgb(220, 220, 220);
            }


            // Recursively apply to child controls (e.g., in a Panel or GroupBox)
            foreach (Control child in control.Controls)
            {
                ApplyDarkThemeToControl(child);
            }
        }
    }
}
