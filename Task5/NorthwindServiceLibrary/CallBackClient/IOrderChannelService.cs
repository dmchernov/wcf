﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NorthwindService.Contracts.ServiceContracts;

namespace CallBackClient
{
	interface IOrderChannelService : IOrderService, IClientChannel
	{
	}
}
