using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session2_Kazan
{
    public partial class EmergencyMaintainenece : Form
    {
        Employee LoggedInUser;
        public EmergencyMaintainenece(Employee e)
        {
            LoggedInUser = e;
            Initialize();
        }
        public async void Initialize()
        {
            var dbtask = GetData();
            InitializeComponent();
            dataGridView1.DataSource = await dbtask;
        }
        public async Task<List<MaintaineneceEmergency>> GetData()
        {
            var returnlist = new List<MaintaineneceEmergency>();
            using (var db = new Session2Entities())
            {
                var listofassets = (from a in db.Assets
                                where a.EmployeeID == a.EmployeeID
                                select a).ToList();
                Parallel.ForEach(listofassets, item=> {
                    if ((from e in db.EmergencyMaintenances where e.AssetID == item.ID select e).Any()) {
                        var me = new MaintaineneceEmergency();
                        me.AssetName = item.AssetName;
                        returnlist.Add(new MaintaineneceEmergency()
                        {
                            AssetName = item.AssetName,
                            AssetSN = item.AssetSN,
                            NumberOfEMs = (from e in db.EmergencyMaintenances where e.ID == item.ID orderby e.EMEndDate descending select e.EMEndDate).Count(),
                            LastClosedEM = (from e in db.EmergencyMaintenances where e.ID == item.ID orderby e.EMEndDate descending select e.EMEndDate).First().ToString()
                        }
                        );
                    }
                    else
                    {
                        returnlist.Add(new MaintaineneceEmergency()
                        {
                            AssetName = item.AssetName,
                            AssetSN = item.AssetSN,
                            NumberOfEMs = (from e in db.EmergencyMaintenances where e.ID == item.ID orderby e.EMEndDate descending select e.EMEndDate).Count(),
                            LastClosedEM = "---"
                        }
                        );
                    }
                });
            }
            return returnlist;
        } 

        private void button1_Click(object sender, EventArgs e)
        {
            //windows forms isnt fun anymore.
            //fucking kill me
        }
    }
    public class MaintaineneceEmergency
    {
        public string AssetSN { get; set; }
        public string AssetName { get; set; }
        public string LastClosedEM { get; set; }
        public int NumberOfEMs { get; set; }
    }
}
