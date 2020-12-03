using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Windows;
using View = Autodesk.Revit.DB.View;

namespace TorsionTools.WPF
{
    /// <summary>
    /// Interaction logic for SheetPreviewWPF.xaml
    /// </summary>
    public partial class SheetPreviewWPF : Window
    {
        Document doc;
        ElementId ViewId;
        public SheetPreviewWPF(Document _doc, ElementId _ViewId)
        {
            InitializeComponent();
            doc = _doc;
            ViewId = _ViewId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (PreviewControl pc = new PreviewControl(doc, ViewId))
                {
                    PreviewGrid.Children.Add(pc);
                    pc.UIView.ZoomToFit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
