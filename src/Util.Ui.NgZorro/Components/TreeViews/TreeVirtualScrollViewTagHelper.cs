﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Ui.NgZorro.Components.TreeViews.Renders;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.TreeViews; 

/// <summary>
/// 虚拟滚动树视图,生成的标签为&lt;nz-tree-virtual-scroll-view>&lt;/nz-tree-virtual-scroll-view>
/// </summary>
[HtmlTargetElement( "util-tree-virtual-scroll-view" )]
public class TreeVirtualScrollViewTagHelper : TreeViewTagHelper {
    /// <summary>
    /// [nzItemSize],节点大小,虚拟滚动时每一列的高度,类型: number, 单位:像素,默认值: 28
    /// </summary>
    public string ItemSize { get; set; }
    /// <summary>
    /// [nzMaxBufferPx],虚拟滚动缓冲区最大高度,需要渲染新节点时的缓冲区大小,类型: number, 单位:像素,默认值: 28 * 10
    /// </summary>
    public string MaxBufferPx { get; set; }
    /// <summary>
    /// [nzMinBufferPx],虚拟滚动缓冲区最小高度，超出渲染区的最小缓存区大小,类型: number, 单位:像素,默认值: 28 * 5
    /// </summary>
    public string MinBufferPx { get; set; }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        var config = new Config( context, output, content );
        return new TreeVirtualScrollViewRender( config );
    }
}