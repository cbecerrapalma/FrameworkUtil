﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Ui.Angular.TagHelpers;
using Util.Ui.NgZorro.Components.Cards.Renders;
using Util.Ui.NgZorro.Enums;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Cards; 

/// <summary>
/// 卡片,生成的标签为&lt;nz-card>&lt;/nz-card>
/// </summary>
[HtmlTargetElement( "util-card")]
public class CardTagHelper : AngularTagHelperBase {
    /// <summary>
    /// nzTitle,标题,支持多语言
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// [nzTitle],标题,类型: string|TemplateRef&lt;void>
    /// </summary>
    public string BindTitle { get; set; }
    /// <summary>
    /// [nzActions],卡片操作组，位于卡片底部,类型: Array&lt;TemplateRef&lt;void>>
    /// </summary>
    public string Actions { get; set; }
    /// <summary>
    /// [nzBodyStyle],内容区域自定义样式,类型: { [key: string]: string }
    /// </summary>
    public string BodyStyle { get; set; }
    /// <summary>
    /// [nzBordered],是否显示边框, 类型: boolean, 默认值: true
    /// </summary>
    public string Bordered { get; set; }
    /// <summary>
    /// [nzCover],卡片封面,类型: TemplateRef&lt;void>
    /// </summary>
    public string Cover { get; set; }
    /// <summary>
    /// [nzExtra],卡片右上角操作区域,类型: string | TemplateRef&lt;void>
    /// </summary>
    public string Extra { get; set; }
    /// <summary>
    /// [nzHoverable],鼠标滑过时是否可浮起, 类型: boolean, 默认值: false
    /// </summary>
    public string Hoverable { get; set; }
    /// <summary>
    /// [nzLoading],是否加载状态, 类型: boolean, 默认值: false
    /// </summary>
    public string Loading { get; set; }
    /// <summary>
    /// nzType,卡片类型，可设置为 inner 或 不设置
    /// </summary>
    public CardType Type { get; set; }
    /// <summary>
    /// [nzType],卡片类型，可设置为 inner 或 不设置
    /// </summary>
    public string BindType { get; set; }
    /// <summary>
    /// nzSize,卡片尺寸，可选值: 'default'|'small',默认值: 'default'
    /// </summary>
    public CardSize Size { get; set; }
    /// <summary>
    /// [nzSize],卡片尺寸，可选值: 'default'|'small',默认值: 'default'
    /// </summary>
    public string BindSize { get; set; }
    /// <summary>
    /// *nzSpaceItem,值为 true 时设置为间距项,放入 &lt;util-space> 中使用
    /// </summary>
    public bool SpaceItem { get; set; }
    /// <summary>
    /// (click),单击事件
    /// </summary>
    public string OnClick { get; set; }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        var config = new Config( context, output, content );
        return new CardRender( config );
    }
}