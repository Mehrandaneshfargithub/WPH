using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.Item;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ItemMvcMockingService : IItemMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public OperationStatus RemoveItem(Guid Itemid)
        {
            try
            {
                Item Hos = _unitOfWork.Items.Get(Itemid);
                _unitOfWork.Items.Remove(Hos);
                _unitOfWork.Complete();
                return OperationStatus.SUCCESSFUL;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity;
                }
                else
                {
                    return OperationStatus.ERROR_SomeThingWentWrong;
                }
            }
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Item/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }
        public string AddNewItem(ItemViewModel item)
        {
            if (string.IsNullOrWhiteSpace(item.ItemTypeName) || string.IsNullOrWhiteSpace(item.Name))
                return "DataNotValid";

            Item item1 = Common.ConvertModels<Item, ItemViewModel>.convertModels(item);

            var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(item.ItemTypeName, "ItemType", item.ClinicSectionId.Value);
            item1.ItemTypeId = typeId;
            

            if (typeId == null || typeId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Name = item.ItemTypeName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("ItemType"),
                    ClinicSectionId = item.ClinicSectionId
                };

                item1.ItemType = baseInfo;
            }

            var sectionId = _unitOfWork.BaseInfos.GetIdByNameAndType(item.SectionName, "Section", item.ClinicSectionId.Value);
            item1.SectionId = sectionId;


            if (sectionId == null || sectionId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Name = item.SectionName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("Section"),
                    ClinicSectionId = item.ClinicSectionId
                };

                item1.Section = baseInfo;
            }


            _unitOfWork.Items.Add(item1);
            _unitOfWork.Complete();
            return item1.Guid.ToString();
        }



        public string UpdateItem(ItemViewModel item)
        {
            if (string.IsNullOrWhiteSpace(item.ItemTypeName) || string.IsNullOrWhiteSpace(item.Name))
                return "DataNotValid";

            var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(item.ItemTypeName, "ItemType", item.ClinicSectionId.Value);
            item.ItemTypeId = typeId;
            Item item1 = Common.ConvertModels<Item, ItemViewModel>.convertModels(item);

            if (typeId == null || typeId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Name = item.ItemTypeName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("ItemType"),
                    ClinicSectionId = item.ClinicSectionId
                };

                _unitOfWork.BaseInfos.Add(baseInfo);

                item1.ItemType = baseInfo;
            }

            _unitOfWork.Items.UpdateState(item1);
            _unitOfWork.Complete();
            return item1.Guid.ToString();

        }

        public bool CheckRepeatedItemName(Guid clinicSectionId, string name, bool NewOrUpdate, string oldName = "")
        {
            try
            {
                Item item = null;
                if (NewOrUpdate)
                {
                    item = _unitOfWork.Items.GetSingle(x => x.Name.Trim() == name.Trim() && x.ClinicSectionId == clinicSectionId);
                }
                else
                {
                    item = _unitOfWork.Items.GetSingle(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName && x.ClinicSectionId == clinicSectionId);
                }
                if (item != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        public IEnumerable<ItemViewModel> GetAllItems(Guid clinicSectionId)
        {
            IEnumerable<Item> hosp = _unitOfWork.Items.GetAllItem(clinicSectionId);
            List<ItemViewModel> hospconvert = ConvertModelsLists(hosp);
            Indexing<ItemViewModel> indexing = new Indexing<ItemViewModel>();
            return indexing.AddIndexing(hospconvert);
        }
        public ItemViewModel GetItem(Guid ItemId)
        {
            try
            {
                Item Itemgu = _unitOfWork.Items.GetWithType(ItemId);
                return ConvertModel(Itemgu);
            }
            catch { return null; }
        }

        // Begin Convert 
        public ItemViewModel ConvertModel(Item Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Item, ItemViewModel>()
                .ForMember(a => a.ItemTypeName, b => b.MapFrom(c => c.ItemType.Name))
                .ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name));
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Item, ItemViewModel>(Users);
        }
        public List<ItemViewModel> ConvertModelsLists(IEnumerable<Item> items)
        {
            List<ItemViewModel> itemDtoList = new List<ItemViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Item, ItemViewModel>()
                .ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name));
            });

            IMapper mapper = config.CreateMapper();
            itemDtoList = mapper.Map<IEnumerable<Item>, List<ItemViewModel>>(items);
            return itemDtoList;
        }
        // End Convert
    }
}
