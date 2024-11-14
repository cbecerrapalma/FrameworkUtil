﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Applications.Trees;
using Util.Ui.Angular.TagHelpers;
using Util.Ui.NgZorro.Components.Trees.Renders;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Trees;

/// <summary>
/// 树形,生成的标签为&lt;nz-tree>&lt;/nz-tree>
/// </summary>
[HtmlTargetElement( "util-tree" )]
public class TreeTagHelper : AngularTagHelperBase {
    /// <summary>
    /// [nzData],数据源,类型: NzTreeNodeOptions[] | NzTreeNode[],默认值: []
    /// </summary>
    public string Data { get; set; }
    /// <summary>
    /// [nzCheckedKeys],指定选中复选框的节点标识列表,类型: string[]
    /// </summary>
    public string CheckedKeys { get; set; }
    /// <summary>
    /// [nzExpandedKeys],指定展开的节点标识列表,类型: string[]
    /// </summary>
    public string ExpandedKeys { get; set; }
    /// <summary>
    /// [nzSelectedKeys],指定选中的节点标识列表,类型: string[]
    /// </summary>
    public string SelectedKeys { get; set; }
    /// <summary>
    /// [nzCheckable],节点前是否显示复选框,类型: boolean, 默认值: false
    /// </summary>
    public string Checkable { get; set; }
    /// <summary>
    /// [nzCheckStrictly],严格勾选,父子节点选中状态不再关联,类型: boolean, 默认值: false
    /// </summary>
    public string CheckStrictly { get; set; }
    /// <summary>
    /// [nzDraggable],节点是否可拖拽,类型: boolean, 默认值: false
    /// </summary>
    public string Draggable { get; set; }
    /// <summary>
    /// [nzMultiple],是否支持点选多个节点,类型: boolean, 默认值: false
    /// </summary>
    public string Multiple { get; set; }
    /// <summary>
    /// [nzExpandAll],是否默认展开所有节点,类型: boolean, 默认值: false
    /// </summary>
    public string ExpandAll { get; set; }
    /// <summary>
    /// [nzBlockNode],节点是否占据一行,类型: boolean, 默认值: false
    /// </summary>
    public string BlockNode { get; set; }
    /// <summary>
    /// [nzAsyncData],是否异步加载,类型: boolean, 默认值: false
    /// </summary>
    public string AsyncData { get; set; }
    /// <summary>
    /// [nzShowLine],是否显示连接线,类型: boolean, 默认值: false
    /// </summary>
    public string ShowLine { get; set; }
    /// <summary>
    /// [nzShowExpand],节点前是否显示展开图标,类型: boolean, 默认值: true
    /// </summary>
    public string ShowExpand { get; set; }
    /// <summary>
    /// [nzShowIcon],是否显示节点文本前图标,没有默认样式,类型: boolean,默认值: false
    /// </summary>
    public string ShowIcon { get; set; }
    /// <summary>
    /// [nzBeforeDrop],拖拽前二次校验函数，决定是否允许放置,类型: (confirm: NzFormatBeforeDropEvent) => Observable&lt;boolean>
    /// </summary>
    public string BeforeDrop { get; set; }
    /// <summary>
    /// [nzExpandedIcon],自定义展开图标,类型: TemplateRef&lt;{ $implicit: NzTreeNode }>
    /// </summary>
    public string ExpandedIcon { get; set; }
    /// <summary>
    /// [nzTreeTemplate],自定义节点模板,类型: TemplateRef&lt;{ $implicit: NzTreeNode }>
    /// </summary>
    public string TreeTemplate { get; set; }
    /// <summary>
    /// nzSearchValue,搜索值,高亮节点,默认值: null
    /// </summary>
    public string SearchValue { get; set; }
    /// <summary>
    /// [nzSearchValue],搜索值,高亮节点,默认值: null
    /// </summary>
    public string BindSearchValue { get; set; }
    /// <summary>
    /// [(nzSearchValue)],搜索值,高亮节点,默认值: null
    /// </summary>
    public string BindonSearchValue { get; set; }
    /// <summary>
    /// [nzSearchFunc],自定义搜索方法，类型: (node: NzTreeNodeOptions) => boolean,默认值: null
    /// </summary>
    public string SearchFunc { get; set; }
    /// <summary>
    /// [nzHideUnMatched],是否隐藏搜索未匹配节点,类型: boolean, 默认值: false
    /// </summary>
    public string HideUnmatched { get; set; }
    /// <summary>
    /// nzVirtualHeight,虚拟滚动的总高度,范例: '300px'
    /// </summary>
    public string VirtualHeight { get; set; }
    /// <summary>
    /// [nzVirtualHeight],虚拟滚动的总高度,范例: '300px'
    /// </summary>
    public string BindVirtualHeight { get; set; }
    /// <summary>
    /// [nzVirtualItemSize],虚拟滚动时每一列的高度,类型: number, 单位:像素, 默认值: 28
    /// </summary>
    public string VirtualItemSize { get; set; }
    /// <summary>
    /// [nzVirtualMaxBufferPx],虚拟滚动缓冲区最大高度,类型: number, 单位:像素,默认值: 500
    /// </summary>
    public string VirtualMaxBufferPx { get; set; }
    /// <summary>
    /// [nzVirtualMinBufferPx],虚拟滚动缓冲区最小高度，低于该值时将加载新结构,类型: number, 单位:像素,默认值: 28
    /// </summary>
    public string VirtualMinBufferPx { get; set; }
    /// <summary>
    /// (nzClick),单击事件,点击树节点时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnClick { get; set; }
    /// <summary>
    /// (nzDblClick),双击事件,双击树节点时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnDblClick { get; set; }
    /// <summary>
    /// (nzContextMenu),上下文菜单事件,右键点击树节点时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public new string OnContextmenu { get; set; }
    /// <summary>
    /// (nzCheckBoxChange),树节点复选框选中状态变化事件,点击树节点复选框时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnCheckboxChange { get; set; }
    /// <summary>
    /// (nzExpandChange),展开收缩节点事件,点击展开树节点图标时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnExpandChange { get; set; }
    /// <summary>
    /// (nzSearchValueChange),搜索值变化事件,搜索节点时触发,与 nzSearchValue 配合使用,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnSearchValueChange { get; set; }
    /// <summary>
    /// (nzOnDragStart),开始拖拽时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnDragStart { get; set; }
    /// <summary>
    /// (nzOnDragEnter),拖拽树节点进入目标容器范围时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnDragEnter { get; set; }
    /// <summary>
    /// (nzOnDragOver),拖拽树节点在目标容器范围内移动时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnDragOver { get; set; }
    /// <summary>
    /// (nzOnDragLeave),拖拽树节点离开目标容器范围时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnDragLeave { get; set; }
    /// <summary>
    /// (nzOnDragEnd),拖拽结束时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnDragEnd { get; set; }
    /// <summary>
    /// (nzOnDrop),鼠标在拖放目标上释放时触发,类型: EventEmitter&lt;NzFormatEmitEvent>
    /// </summary>
    public string OnDrop { get; set; }
    /// <summary>
    /// 扩展属性,是否启用扩展指令,当设置Api地址时自动启用
    /// </summary>
    public bool EnableExtend { get; set; }
    /// <summary>
    /// 扩展属性 [autoLoad],初始化时是否自动加载数据，默认为true,设置成false则手工加载
    /// </summary>
    public bool AutoLoad { get; set; }
    /// <summary>
    /// 扩展属性 url,Api地址
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// 扩展属性 [url],Api地址
    /// </summary>
    public string BindUrl { get; set; }
    /// <summary>
    /// 扩展属性 loadUrl,首次加载地址，对于异步请求,仅加载第一级节点,如果未设置该属性,则使用 url 地址
    /// </summary>
    public string LoadUrl { get; set; }
    /// <summary>
    /// 扩展属性 [loadUrl],首次加载地址，对于异步请求,仅加载第一级节点,如果未设置该属性,则使用 url 地址
    /// </summary>
    public string BindLoadUrl { get; set; }
    /// <summary>
    /// 扩展属性 queryUrl,查询地址，如果未设置该属性,则使用 url 地址
    /// </summary>
    public string QueryUrl { get; set; }
    /// <summary>
    /// 扩展属性 [queryUrl],查询地址，如果未设置该属性,则使用 url 地址
    /// </summary>
    public string BindQueryUrl { get; set; }
    /// <summary>
    /// 扩展属性 loadChildrenUrl,加载子节点地址，如果未设置该属性,则使用 url 地址
    /// </summary>
    public string LoadChildrenUrl { get; set; }
    /// <summary>
    /// 扩展属性 [loadChildrenUrl],加载子节点地址，如果未设置该属性,则使用 url 地址
    /// </summary>
    public string BindLoadChildrenUrl { get; set; }
    /// <summary>
    /// 扩展属性 loadMode,加载模式，默认为同步加载
    /// </summary>
    public LoadMode LoadMode { get; set; }
    /// <summary>
    /// 扩展属性 [isExpandForRootAsync],根节点异步加载模式是否展开子节点,默认为 true
    /// </summary>
    public bool ExpandForRootAsync { get; set; }
    /// <summary>
    /// 扩展属性[(queryParam)],查询参数
    /// </summary>
    public string QueryParam { get; set; }
    /// <summary>
    /// 扩展属性 order,排序条件,范例: creationTime desc
    /// </summary>
    public string Sort { get; set; }
    /// <summary>
    /// 扩展属性 [order],排序条件,范例: creationTime desc
    /// </summary>
    public string BindSort { get; set; }
    /// <summary>
    /// 扩展属性 [onLoadChildrenBefore],子节点加载前事件,返回false停止加载,类型: (parentNode:NzTreeNode,queryParam) => boolean,参数为父节点和查询参数
    /// </summary>
    public string OnLoadChildrenBefore { get; set; }
    /// <summary>
    /// 扩展事件 (onLoadChildren),子节点加载完成事件,类型: EventEmitter&lt;{ node: NzTreeNode, result }>
    /// </summary>
    public string OnLoadChildren { get; set; }
    /// <summary>
    /// 扩展事件 (onExpand),节点展开事件,类型: EventEmitter&lt;NzTreeNode>
    /// </summary>
    public string OnExpand { get; set; }
    /// <summary>
    /// 扩展事件 (onCollapse),节点折叠事件,类型: EventEmitter&lt;NzTreeNode>
    /// </summary>
    public string OnCollapse { get; set; }
    /// <summary>
    /// 扩展事件 (onLoad),数据加载完成事件,参数为服务端返回结果
    /// </summary>
    public string OnLoad { get; set; }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        var config = new Config( context, output, content );
        return new TreeRender( config );
    }
}