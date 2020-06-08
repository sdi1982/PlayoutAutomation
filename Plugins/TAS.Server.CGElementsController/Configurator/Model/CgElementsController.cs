﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TAS.Common;
using TAS.Common.Interfaces;
using TAS.Database.Common;

namespace TAS.Server.CgElementsController.Configurator.Model
{    
    public class CgElementsController : ICGElementsController
    {   
        [Hibernate]
        public List<string> Startup { get; set; }
        [Hibernate]
        [JsonConverter(typeof(ConcreteListConverter<ICGElement, CgElement>))]
        public IEnumerable<ICGElement> Logos { get; set; }
        [Hibernate]
        [JsonConverter(typeof(ConcreteListConverter<ICGElement, CgElement>))]
        public IEnumerable<ICGElement> Parentals { get; set; }
        [Hibernate]
        [JsonConverter(typeof(ConcreteListConverter<ICGElement, CgElement>))]
        public IEnumerable<ICGElement> Crawls { get; set; }
        [Hibernate]
        [JsonConverter(typeof(ConcreteListConverter<ICGElement, CgElement>))]
        public IEnumerable<ICGElement> Auxes { get; set; }
        [Hibernate]
        public bool IsEnabled { get; set; }
        public byte DefaultCrawl { get; set; }
        public byte DefaultLogo { get; set; }        

        public CgElementsController()
        {
            Crawls = new List<ICGElement>();
            Logos = new List<ICGElement>();
            Parentals = new List<ICGElement>();
            Auxes = new List<ICGElement>();
            Startup = new List<string>();
        }

        #region ICGElementsController
        public bool IsMaster => throw new NotImplementedException();

        public bool IsConnected => throw new NotImplementedException();

        public bool IsCGEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public byte Crawl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public byte Logo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public byte Parental { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsWideScreen { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Started;

        public void SetState(ICGElementsState state)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
