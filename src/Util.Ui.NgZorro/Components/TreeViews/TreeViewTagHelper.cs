﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Ui.Angular.TagHelpers;
using Util.Ui.NgZorro.Components.TreeViews.Renders;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.TreeViews; 

/// <summary>
/// 树视图,生成的标签为&lt;nz-tree-view>&lt;/nz-tree-view>
/// </summary>
[HtmlTargetElement( "util-tree-view" )]
public class TreeViewTagHelper : AngularTagHelperBase {
    /// <summary>
    /// [nzTreeControl],树控制器,类型: TreeControl
    /// </summary>
    public string TreeControl { get; set; }
    /// <summary>
    /// [nzDataSource],数据源,数组数据,类型: DataSource&lt;T> | Observable&lt;T[]> | T[]
    /// </summary>
    public string Datasource { get; set; }
    /// <summary>
    /// [nzDirectoryTree],是否以文件夹样式显示节点,类型: boolean, 默认值: false
    /// </summary>
    public string DirectoryTree { get; set; }
    /// <summary>
    /// [nzBlockNode],节点是否占据整行,类型: boolean, 默认值: false
    /// </summary>
    public string BlockNode { get; set; }
    /// <summary>
    /// [trackBy],跟踪函数
    /// </summary>
    public string TrackBy { get; set; }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        var config = new Config( context, output, content );
        return new TreeViewRender( config );
    }
}