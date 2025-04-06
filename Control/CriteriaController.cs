/**
 * Copyright (C) 2025 Xibo Signage Ltd
 *
 * Xibo - Digital Signage - https://xibosignage.com
 *
 * This file is part of Xibo.
 *
 * Xibo is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * Xibo is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with Xibo.  If not, see <http://www.gnu.org/licenses/>.
 */
using EmbedIO.Routing;
using EmbedIO;
using EmbedIO.WebApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XiboClient.Control
{
    internal class CriteriaController : WebApiController
    {
        private EmbeddedServer _parent;

        public CriteriaController(EmbeddedServer parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Trigger some action.
        /// </summary>
        [Route(HttpVerbs.Post, "/")]
        public async void Criteria()
        {
            try
            {
                var resolvedItems = new List<CriteriaRequest>();
                var data = await HttpContext.GetRequestBodyAsStringAsync();
                var items = JsonConvert.DeserializeObject<JArray>(data);
                foreach (var item in items.Children())
                {
                    resolvedItems.Add(new CriteriaRequest
                    {
                        metric = item["metric"].ToString(),
                        value = item["value"].ToString(),
                        ttl = int.Parse(item["ttl"].ToString())
                    });
                }
                _parent.Criteria(resolvedItems);
            }
            catch (Exception e)
            {
                Trace.WriteLine(new LogMessage("CriteriaController", "Criteria: unable to parse request: " + e.Message), LogType.Error.ToString());
            }
        }
    }
}
