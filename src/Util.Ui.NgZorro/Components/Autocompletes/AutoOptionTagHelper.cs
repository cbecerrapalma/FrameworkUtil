﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Ui.Angular.TagHelpers;
using Util.Ui.NgZorro.Components.Autocompletes.Renders;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Autocompletes; 

/// <summary>
/// 自动完成项,生成的标签为&lt;nz-auto-option&gt;&lt;/nz-auto-option&gt;
/// </summary>
[HtmlTargetElement( "util-auto-option" )]
public class AutoOptionTagHelper : AngularTagHelperBase {
    /// <summary>
    /// nzValue,绑定到触发元素 ngModel 的值
    /// </summary>
    public string Value { get; set; }
    /// <summary>
    /// [nzValue],绑定到触发元素 ngModel 的值
    /// </summary>
    public string BindValue { get; set; }
    /// <summary>
    /// nzLabel,填入触发元素显示的值
    /// </summary>
    public string Label { get; set; }
    /// <summary>
    /// [nzLabel],填入触发元素显示的值
    /// </summary>
    public string BindLabel { get; set; }
    /// <summary>
    /// [nzDisabled],禁用选项, 默认值: false
    /// </summary>
    public string Disabled { get; set; }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        var config = new Config( context, output, content );
        return new AutoOptionRender( config );
    }
}