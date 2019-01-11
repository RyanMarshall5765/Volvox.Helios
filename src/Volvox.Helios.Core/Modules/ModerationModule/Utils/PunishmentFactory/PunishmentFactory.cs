﻿using System;
using System.Collections.Generic;
using System.Text;
using Volvox.Helios.Core.Modules.ModerationModule.PunishmentService.Punishments;
using Volvox.Helios.Domain.Module.ModerationModule.Common;

namespace Volvox.Helios.Core.Modules.ModerationModule.Utils.PunishmentFactory
{
    public class PunishmentFactory : IPunishmentFactory
    {
        private readonly Dictionary<PunishType, IPunishment> _punishments = new Dictionary<PunishType, IPunishment>();

        public PunishmentFactory(IEnumerable<IPunishment> punishments)
        {
            foreach (var punishment in punishments)
            {
                var punishmentType = punishment.GetPunishmentMetaData().PunishType;

                _punishments[punishmentType] = punishment;
            }
        }

        public IPunishment GetPunishment(PunishType type)
        {
            return _punishments[type];
        }
    }
}
