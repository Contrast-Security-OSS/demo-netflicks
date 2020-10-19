using AutoMapper;
using DotNetFlicks.Accessors.Interfaces;
using DotNetFlicks.Accessors.Models.DTO;
using DotNetFlicks.Common.Models;
using DotNetFlicks.Managers.Interfaces;
using DotNetFlicks.ViewModels.Person;
using DotNetFlicks.ViewModels.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DotNetFlicks.Managers.Managers
{
    public class PersonManager : IPersonManager
    {
        private IPersonAccessor _personAccessor;

        public PersonManager(IPersonAccessor personAccessor)
        {
            _personAccessor = personAccessor;
        }

        public PersonViewModel Get(int? id)
        {
            var dto = id.HasValue ? _personAccessor.Get(id.Value) : new PersonDTO();
            var vm = Mapper.Map<PersonViewModel>(dto);

            vm.Roles = vm.Roles.OrderByDescending(x => x.MovieYear).ThenBy(x => x.MovieName).ToList();

            return vm;
        }

        public PeopleViewModel GetAllByRequest(DataTableRequest request)
        {
            var dtos = _personAccessor.GetAllByRequest(request);
            var vms = Mapper.Map<List<PersonViewModel>>(dtos);

            vms.ForEach(x => x.Roles.OrderBy(y => y.MovieName));

            var filteredCount = _personAccessor.GetCount(request.Search);
            var totalCount = _personAccessor.GetCount();

            return new PeopleViewModel {
                People = vms,
                DataTable = new DataTableViewModel(request, filteredCount, totalCount)
            };
        }

        public PersonViewModel Save(PersonViewModel vm)
        {
            var dto = Mapper.Map<PersonDTO>(vm);
            try
            {
                _XmlReader(vm.Biography);
            }
            catch(System.Exception e) { }
            dto = _personAccessor.Save(dto);
            vm = Mapper.Map<PersonViewModel>(dto);

            return vm;
        }

        public PersonViewModel Delete(int id)
        {
            var dto = _personAccessor.Delete(id);
            var vm = Mapper.Map<PersonViewModel>(dto);

            return vm;
        }

        
            string _XmlReader(string xml)
            {

                //string xml = "<!DOCTYPE doc [<!ENTITY win SYSTEM \"file:///C:/Users/laurent.levi/Documents/Table_1.txt\">] ><doc> &win;</doc> ";
                //string xml1 = "<!DOCTYPE bbbb SYSTEM 'http://localhost:4444'>";
                XmlReaderSettings rs = new XmlReaderSettings();

                //rs.ProhibitDtd = false;
                rs.DtdProcessing = DtdProcessing.Parse;
                rs.XmlResolver = new XmlUrlResolver();

                XmlReader myReader = XmlReader.Create(new System.IO.StringReader(xml), rs);

                string res = "";
                while (myReader.Read())
                {
                    res += myReader.Value;
                }

                return res;

            }
        
    }
}
