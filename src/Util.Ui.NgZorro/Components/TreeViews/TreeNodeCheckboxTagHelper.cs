﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Ui.Angular.TagHelpers;
using Util.Ui.NgZorro.Components.TreeViews.Renders;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.TreeViews;

/// <summary>
/// 树视图节点复选框,生成的标签为&lt;nz-tree-node-checkbox>&lt;/nz-tree-node-checkbox>
/// </summary>
[HtmlTargetElement( "util-tree-node-checkbox" )]
public class TreeNodeCheckboxTagHelper : AngularTagHelperBase {
    /// <summary>
    /// [nzChecked],是否勾选,类型: boolean, 默认值: false
    /// </summary>
    public string Checked { get; set; }
    /// <summary>
    /// [nzIndeterminate],是否半选,类型: boolean, 默认值: false
    /// </summary>
    public string Indeterminate { get; set; }
    /// <summary>
    /// [nzDisabled],是否禁用,类型: boolean, 默认值: false
    /// </summary>
    public string Disabled { get; set; }
    /// <summary>
    /// (nzClick),单击事件,类型: EventEmitter&lt;MouseEvent>
    /// </summary>
    public string OnClick { get; set; }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        var config = new Config( context, output, content );
        return new TreeNodeCheckboxRender( config );
    }
}