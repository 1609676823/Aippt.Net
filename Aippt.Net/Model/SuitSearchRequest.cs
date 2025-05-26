using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aippt.Net.Model
{
    /// <summary>
    /// 模板套装列表请求参数
    /// </summary>
    public class SuitSearchRequest
    {
        /// <summary>
        /// 模板套装列表请求参数
        /// </summary>
        public SuitSearchRequest() { }
        /// <summary>
        /// 模板场景ID
        /// </summary>
        public long? scene_id { get; set; }

        /// <summary>
        /// 风格ID
        /// </summary>
        public long? style_id { get; set; }

        /// <summary>
        /// 颜色ID
        /// </summary>
        public long? colour_id { get; set; }

        /// <summary>
        /// 页码 默认1
        /// </summary>
        public long? page { get; set; }

        /// <summary>
        /// 每页展示数量 默认20
        /// </summary>
        public long? page_size { get; set; }
    }
}
