﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Ui.Angular.TagHelpers;
using Util.Ui.NgZorro.Components.Collapses.Renders;
using Util.Ui.NgZorro.Enums;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Collapses; 

/// <summary>
/// 折叠,生成的标签为&lt;nz-collapse>&lt;/nz-collapse>
/// </summary>
[HtmlTargetElement( "util-collapse" )]
public class CollapseTagHelper : AngularTagHelperBase {
    /// <summary>
    /// [nzAccordion],是否手风琴,即只能展开一个面板,类型: boolean, 默认值: false
    /// </summary>
    public string Accordion { get; set; }
    /// <summary>
    /// [nzBordered],是否显示边框,类型: boolean, 默认值: true
    /// </summary>
    public string Bordered { get; set; }
    /// <summary>
    /// [nzGhost],是否使折叠面板透明且无边框,类型: boolean, 默认值: false
    /// </summary>
    public string Ghost { get; set; }
    /// <summary>
    /// nzExpandIconPosition,折叠图标位置,可选值: 'left' | 'right' ,默认值: 'left'
    /// </summary>
    public CollapseIconPosition ExpandIconPosition { get; set; }
    /// <summary>
    /// [nzExpandIconPosition],折叠图标位置,可选值: 'left' | 'right' ,默认值: 'left'
    /// </summary>
    public string BindExpandIconPosition { get; set; }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        var config = new Config( context, output, content );
        return new CollapseRender( config );
    }
}