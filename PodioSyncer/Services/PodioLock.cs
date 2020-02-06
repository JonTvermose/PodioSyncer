using PodioSyncer.Models.Podio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Services
{
  public static class PodioLock
  {
    public static readonly Object Lock = new Object();       

  }
}
