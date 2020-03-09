using Autodesk.Revit.DB;
using System;

namespace Revit_2020_Add_In.Updaters
{
    public class ViewSheetUpdater : IUpdater
    {
        static AddInId m_appId;
        static UpdaterId m_updaterId;

        public ViewSheetUpdater(AddInId id)
        {
            m_appId = id;
            m_updaterId = new UpdaterId(m_appId, new Guid("1BD20525-4471-49A1-9D29-3351436DFB2C"));
        }
        public void Execute(UpdaterData data)
        {
            Document doc = data.GetDocument();
            foreach (ElementId addedElemId in data.GetAddedElementIds())
            {
                ViewSheet sheet = doc.GetElement(addedElemId) as ViewSheet;
                using (Forms.ViewSheetUpdaterForm form = new Forms.ViewSheetUpdaterForm(doc, sheet))
                {
                    form.ShowDialog();
                }
            }
        }

        public string GetAdditionalInformation()
        {
            return "Check to see when Sheets are added";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Views;
        }

        public UpdaterId GetUpdaterId()
        {
            return m_updaterId;
        }

        public string GetUpdaterName()
        {
            return "Torsion Tools Sheet Information Updater";
        }

    }
}
