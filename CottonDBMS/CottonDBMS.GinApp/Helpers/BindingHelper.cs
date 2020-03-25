//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.DataModels;

namespace CottonDBMS.GinApp.Helpers
{
    public static class BindingHelper
    {
        public static void BindModuleStatusCombo(ComboBox cbo, string firstItemText, ModuleStatus? selectedStatus=null)
        {
            cbo.Items.Clear();
            cbo.DisplayMember = "Name";
            cbo.Items.Add(new BaseEntity { Id = "0", Name = firstItemText });
            cbo.Items.Add(new BaseEntity { Id = Convert.ToInt32(ModuleStatus.IN_FIELD).ToString(), Name = "In field" });
            cbo.Items.Add(new BaseEntity { Id = Convert.ToInt32(ModuleStatus.PICKED_UP).ToString(), Name = "Picked up" });
            cbo.Items.Add(new BaseEntity { Id = Convert.ToInt32(ModuleStatus.AT_GIN).ToString(), Name = "At Gin" });
            cbo.Items.Add(new BaseEntity { Id = Convert.ToInt32(ModuleStatus.ON_FEEDER).ToString(), Name = "On feeder" });
            cbo.Items.Add(new BaseEntity { Id = Convert.ToInt32(ModuleStatus.GINNED).ToString(), Name = "Ginned" });


            if (selectedStatus.HasValue)
            {
                BaseEntity selectedItem = null;
                foreach (BaseEntity item in cbo.Items)
                {
                    if (Convert.ToInt32(item.Id) == Convert.ToInt32(selectedStatus.Value))
                    {
                        selectedItem = item;
                    }
                }

                cbo.SelectedItem = selectedItem;
            }
            else
            {
                cbo.SelectedIndex = 0;
            }
        }

        public static void BindPickupListStatusCombo(ComboBox cbo, string firstItemText, PickupListStatus? selectedStatus = null)
        {
            cbo.Items.Clear();
            cbo.DisplayMember = "Name";
            cbo.Items.Add(new BaseEntity { Id = "0", Name = firstItemText });
            cbo.Items.Add(new BaseEntity { Id = Convert.ToInt32(PickupListStatus.OPEN).ToString(), Name = "Open" });
            cbo.Items.Add(new BaseEntity { Id = Convert.ToInt32(PickupListStatus.COMPLETE).ToString(), Name = "Complete" });

            if (selectedStatus.HasValue)
            {
                BaseEntity selectedItem = null;
                foreach (BaseEntity item in cbo.Items)
                {
                    if (Convert.ToInt32(item.Id) == Convert.ToInt32(selectedStatus.Value))
                    {
                        selectedItem = item;
                    }
                }

                cbo.SelectedItem = selectedItem;
            }
            else
            {
                cbo.SelectedIndex = 0;
            }
        }

        public static void BindPickupListDestinationCombo(ComboBox cbo, PickupListDestination? selectedDestination = null)
        {
            
            cbo.Items.Clear();
            cbo.DisplayMember = "Name";
            //cbo.Items.Add(new BaseEntity { Id = "0", Name = firstItemText });
            cbo.Items.Add(new BaseEntity { Id = Convert.ToInt32(PickupListDestination.GIN_YARD).ToString(), Name = "Gin Yard" });
            cbo.Items.Add(new BaseEntity { Id = Convert.ToInt32(PickupListDestination.GIN_FEEDER).ToString(), Name = "Gin Feeder" });

            if (selectedDestination.HasValue)
            {
                BaseEntity selectedItem = null;
                foreach (BaseEntity item in cbo.Items)
                {
                    if (Convert.ToInt32(item.Id) == Convert.ToInt32(selectedDestination.Value))
                    {
                        selectedItem = item;
                    }
                }

                cbo.SelectedItem = selectedItem;
            }
            else
            {
                cbo.SelectedIndex = 0;
            }
        }

        public static void BindTruckComboByName(ComboBox cbo, string firstItemText, string name = "")
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                cbo.Items.Clear();
                cbo.DisplayMember = "Name";
                var items = uow.TruckRepository.GetAll();
                cbo.Items.Add(new BaseEntity { Id = "0", Name = firstItemText });
                foreach (var item in items.OrderBy(t => t.Name))
                {
                    cbo.Items.Add(item);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    BaseEntity selectedItem = null;
                    foreach (BaseEntity item in cbo.Items)
                    {
                        if (item.Name == name)
                        {
                            selectedItem = item;
                        }
                    }
                    cbo.SelectedItem = selectedItem;
                }
                else
                {
                    cbo.SelectedIndex = 0;
                }
            }
        }

        public static void BindDriverComboByName(ComboBox cbo, string firstItemText, string name = "")
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                cbo.Items.Clear();
                cbo.DisplayMember = "Name";
                var items = uow.DriverRepository.GetAll();
                cbo.Items.Add(new BaseEntity { Id = "0", Name = firstItemText });
                foreach (var item in items.OrderBy(t => t.Name))
                {
                    cbo.Items.Add(item);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    BaseEntity selectedItem = null;
                    foreach (BaseEntity item in cbo.Items)
                    {
                        if (item.Name == name)
                        {
                            selectedItem = item;
                        }
                    }
                    cbo.SelectedItem = selectedItem;
                }
                else
                {
                    cbo.SelectedIndex = 0;
                }
            }
        }
    }
}
