using System;
using System.Collections.Generic;
using System.Linq;
using XmlSourceMicroservice.Models;

namespace XmlSourceMicroservice.Data
{
    public class Repository
    {
        private IEnumerable<XmlModel> _dataRepository { get; set; }

        public Repository()
        {
            _dataRepository = new[]
            {
                new XmlModel
                {
                    Temperature = 5.6,
                    Pressure = 2002.21
                },
                new XmlModel
                {
                    Temperature = 7.9,
                    Pressure = 2022.31
                },
                new XmlModel
                {
                    Temperature = 0.6,
                    Pressure = 2222.41
                },
                new XmlModel
                {
                    Temperature = -5.6,
                    Pressure = 22222.99
                },
            };
        }

        public XmlModel GetData()
        {
            XmlModel randomValue = _dataRepository.OrderBy(x => new Random().Next()).First();
            return randomValue;
        }
    }
}
