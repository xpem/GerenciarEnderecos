using Domain;
using Domain.DTOs;
using Domain.Requests;
using Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Tests
{
    [TestClass()]
    public class AddressServiceTests
    {
        [TestMethod()]
        public async Task UpdateAsyncTest()
        {
            string model = "{\"Id\":18,\"CEP\":\"31015025\",\"Number\":\"334\",\"Street\":\"Rua Pouso Alegre\",\"Complement\":null,\"Neighborhood\":\"Horto\",\"City\":\"Belo Horizonte\",\"State\":\"MG\"}";
            string modelAddressById = "{\"CEP\":\"31015025\",\"Number\":333,\"Street\":\"Rua Pouso Alegre\",\"Complement\":null,\"Neighborhood\":\"Horto\",\"City\":\"Belo Horizonte\",\"State\":\"MG\",\"UserId\":1,\"User\":null,\"Id\":18,\"CreatedAt\":\"2024-07-21T10:48:03.679371\"}";
            AddressRequest request = JsonConvert.DeserializeObject<AddressRequest>(model);

            Address AddressById = JsonConvert.DeserializeObject<Address>(modelAddressById);

            Mock<IAddressRepo> service = new Mock<IAddressRepo>();

            service.Setup(x => x.Update(It.IsAny<Address>()));
            service.Setup(x => x.GetByIdAsync(18, 1)).ReturnsAsync(AddressById);

            AddressService addressService = new AddressService(service.Object);

            var resp = await addressService.UpdateAsync(request, 1);

            if (resp.Success == true) Assert.IsTrue(true);
            else
                Assert.Fail();
        }

        [TestMethod()]
        public async Task Try_Update_Invalid_State_AsyncTest()
        {
            //state with 3 chars
            string model = "{\"Id\":18,\"CEP\":\"31015025\",\"Number\":\"334\",\"Street\":\"Rua Pouso Alegre\",\"Complement\":null,\"Neighborhood\":\"Horto\",\"City\":\"Belo Horizonte\",\"State\":\"MGE\"}";

            string modelAddressById = "{\"CEP\":\"31015025\",\"Number\":333,\"Street\":\"Rua Pouso Alegre\",\"Complement\":null,\"Neighborhood\":\"Horto\",\"City\":\"Belo Horizonte\",\"State\":\"MG\",\"UserId\":1,\"User\":null,\"Id\":18,\"CreatedAt\":\"2024-07-21T10:48:03.679371\"}";

            AddressRequest request = JsonConvert.DeserializeObject<AddressRequest>(model);

            Address AddressById = JsonConvert.DeserializeObject<Address>(modelAddressById);

            Mock<IAddressRepo> service = new Mock<IAddressRepo>();

            service.Setup(x => x.Update(It.IsAny<Address>()));
            service.Setup(x => x.GetByIdAsync(18, 1)).ReturnsAsync(AddressById);

            AddressService addressService = new AddressService(service.Object);

            var resp = await addressService.UpdateAsync(request, 1);

            if (resp.Error is not null) Assert.IsTrue(true);
            else
                Assert.Fail();
        }
    }
}