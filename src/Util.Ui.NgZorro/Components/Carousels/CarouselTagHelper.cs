﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Ui.Angular.TagHelpers;
using Util.Ui.NgZorro.Components.Carousels.Renders;
using Util.Ui.NgZorro.Enums;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Carousels; 

/// <summary>
/// 走马灯,生成的标签为&lt;nz-carousel>&lt;/nz-carousel>
/// </summary>
[HtmlTargetElement( "util-carousel" )]
public class CarouselTagHelper : AngularTagHelperBase {
    /// <summary>
    /// [nzAutoPlay],是否自动切换,类型: boolean, 默认值: false
    /// </summary>
    public string AutoPlay { get; set; }
    /// <summary>
    /// [nzAutoPlaySpeed],切换时间, 单位:毫秒，当设置为 0 时不切换,默认值: 3000
    /// </summary>
    public string AutoPlaySpeed { get; set; }
    /// <summary>
    /// [nzDotRender],指示点渲染模板,类型: TemplateRef&lt;{ $implicit: number }>
    /// </summary>
    public string DotRender { get; set; }
    /// <summary>
    /// nzDotPosition,指示点位置,可选值: 'top' | 'bottom' | 'left' | 'right',默认值: 'bottom'
    /// </summary>
    public CarouselDotPosition DotPosition { get; set; }
    /// <summary>
    /// [nzDotPosition],指示点位置,可选值: 'top' | 'bottom' | 'left' | 'right',默认值: 'bottom'
    /// </summary>
    public string BindDotPosition { get; set; }
    /// <summary>
    /// [nzDots],是否显示指示点,类型: boolean, 默认值: true
    /// </summary>
    public string Dots { get; set; }
    /// <summary>
    /// nzEffect,动画效果,可选值: 'scrollx' | 'fade',默认值: 'scrollx'
    /// </summary>
    public CarouselEffect Effect { get; set; }
    /// <summary>
    /// [nzEffect],动画效果,可选值: 'scrollx' | 'fade',默认值: 'scrollx'
    /// </summary>
    public string BindEffect { get; set; }
    /// <summary>
    /// [nzEnableSwipe],是否支持手势划动切换,类型: boolean, 默认值: true
    /// </summary>
    public string EnableSwipe { get; set; }
    /// <summary>
    /// [nzLoop],是否支持循环,类型: boolean, 默认值: true
    /// </summary>
    public string Loop { get; set; }
    /// <summary>
    /// (nzBeforeChange),切换面板前事件,类型: EventEmitter&lt;{ from: number; to: number }>
    /// </summary>
    public string OnBeforeChange { get; set; }
    /// <summary>
    /// (nzAfterChange),切换面板事件,类型: EventEmitter&lt;number>
    /// </summary>
    public string OnAfterChange { get; set; }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        var config = new Config( context, output, content );
        return new CarouselRender( config );
    }
}