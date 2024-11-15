﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Ui.Angular.TagHelpers;
using Util.Ui.NgZorro.Components.Tabs.Renders;
using Util.Ui.NgZorro.Enums;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Tabs; 

/// <summary>
/// 标签页,生成的标签为&lt;nz-tabset>&lt;/nz-tabset>
/// </summary>
[HtmlTargetElement( "util-tabset" )]
public class TabSetTagHelper : AngularTagHelperBase {
    /// <summary>
    /// [nzSelectedIndex],选中标签索引,即当前激活标签面板的序号,类型: number
    /// </summary>
    public string SelectedIndex { get; set; }
    /// <summary>
    /// [(nzSelectedIndex)],选中标签索引,即当前激活标签面板的序号,类型: number
    /// </summary>
    public string BindonSelectedIndex { get; set; }
    /// <summary>
    /// [nzAnimated],是否使用动画切换选项卡,类型: boolean | {inkBar:boolean, tabPane:boolean},默认值: true, 当 nzType 为 'card' 时为 false
    /// </summary>
    public string Animated { get; set; }
    /// <summary>
    /// nzSize,标签尺寸,可选值: 'large' | 'small' | 'default',默认值: 'default'
    /// </summary>
    public TabSize Size { get; set; }
    /// <summary>
    /// [nzSize],标签尺寸,可选值: 'large' | 'small' | 'default',默认值: 'default'
    /// </summary>
    public string BindSize { get; set; }
    /// <summary>
    /// [nzTabBarExtraContent],标签栏附加内容,显示在标签栏右边,类型: TemplateRef&lt;void>
    /// </summary>
    public string TabBarExtraContent { get; set; }
    /// <summary>
    /// [nzTabBarStyle],标签栏样式,类型: object
    /// </summary>
    public string TabBarStyle { get; set; }
    /// <summary>
    /// nzTabPosition,标签位置,可选值: 'top' | 'right' | 'bottom' | 'left',默认值: 'top'
    /// </summary>
    public TabPosition TabPosition { get; set; }
    /// <summary>
    /// [nzTabPosition],标签位置,可选值: 'top' | 'right' | 'bottom' | 'left',默认值: 'top'
    /// </summary>
    public string BindTabPosition { get; set; }
    /// <summary>
    /// nzType,标签类型,可选值: 'line' | 'card' | 'editable-card',默认值: 'line'
    /// </summary>
    public TabType Type { get; set; }
    /// <summary>
    /// [nzType],标签类型,可选值: 'line' | 'card' | 'editable-card',默认值: 'line'
    /// </summary>
    public string BindType { get; set; }
    /// <summary>
    /// [nzTabBarGutter],标签间隙, 类型: number
    /// </summary>
    public string TabBarGutter { get; set; }
    /// <summary>
    /// [nzHideAll],是否隐藏全部标签,默认值: false
    /// </summary>
    public string HideAll { get; set; }
    /// <summary>
    /// [nzLinkRouter],是否支持路由联动,默认值: false
    /// </summary>
    public string LinkRouter { get; set; }
    /// <summary>
    /// [nzLinkExact],是否以严格模式匹配路由联动,默认值: true
    /// </summary>
    public string LinkExact { get; set; }
    /// <summary>
    /// [nzCanDeactivate],标签守卫函数,决定标签是否可以被切换,类型: NzTabsCanDeactivateFn
    /// </summary>
    public string CanDeactivate { get; set; }
    /// <summary>
    /// [nzCentered],标签是否居中显示,默认值: false
    /// </summary>
    public string Centered { get; set; }
    /// <summary>
    /// [nzHideAdd],是否隐藏添加按钮,当 nzType 为 'editable-card' 时有效,默认值: false
    /// </summary>
    public string HideAdd { get; set; }
    /// <summary>
    /// nzAddIcon,添加按钮图标,当 nzType 为 'editable-card' 时有效,类型: string | TemplateRef&lt;void>
    /// </summary>
    public AntDesignIcon AddIcon { get; set; }
    /// <summary>
    /// [nzAddIcon],添加按钮图标,当 nzType 为 'editable-card' 时有效,类型: string | TemplateRef&lt;void>
    /// </summary>
    public string BindAddIcon { get; set; }
    /// <summary>
    /// nzCloseIcon,关闭按钮图标,当 nzType 为 'editable-card' 时有效,类型: string | TemplateRef&lt;void>
    /// </summary>
    public AntDesignIcon CloseIcon { get; set; }
    /// <summary>
    /// [nzCloseIcon],关闭按钮图标,当 nzType 为 'editable-card' 时有效,类型: string | TemplateRef&lt;void>
    /// </summary>
    public string BindCloseIcon { get; set; }
    /// <summary>
    /// (nzSelectedIndexChange),选中标签索引变化事件,类型: EventEmitter&lt;number>
    /// </summary>
    public string OnSelectedIndexChange { get; set; }
    /// <summary>
    /// (nzSelectChange),选中标签变化事件,类型: EventEmitter&lt;{index: number,tab: NzTabComponent}>
    /// </summary>
    public string OnSelectChange { get; set; }
    /// <summary>
    /// (nzAdd),添加标签事件,点击添加按钮时触发, nzType='editable-card' 时有效
    /// </summary>
    public string OnAdd { get; set; }
    /// <summary>
    /// (nzClose),关闭标签事件,点击删除按钮时触发, nzType='editable-card' 时有效, 类型: EventEmitter&lt;{ index: number }>
    /// </summary>
    public string OnClose { get; set; }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        var config = new Config( context, output, content );
        return new TabSetRender( config );
    }
}