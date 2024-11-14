﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Ui.NgZorro.Components.Base;
using Util.Ui.NgZorro.Components.Icons.Renders;
using Util.Ui.NgZorro.Enums;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Icons; 

/// <summary>
/// 图标,生成的标签为&lt;span nz-icon>&lt;/span>
/// </summary>
[HtmlTargetElement( "util-icon" )]
public class IconTagHelper : TooltipTagHelperBase {
    /// <summary>
    /// nzType,图标类型
    /// </summary>
    public AntDesignIcon Type { get; set; }
    /// <summary>
    /// [nzType],图标类型
    /// </summary>
    public string BindType { get; set; }
    /// <summary>
    /// nzTheme,图标主题
    /// </summary>
    public IconTheme Theme { get; set; }
    /// <summary>
    /// [nzTheme],图标主题
    /// </summary>
    public string BindTheme { get; set; }
    /// <summary>
    /// [nzSpin],持续旋转
    /// </summary>
    public string Spin { get; set; }
    /// <summary>
    /// [nzRotate],旋转角度
    /// </summary>
    public string Rotate { get; set; }
    /// <summary>
    /// nzTwotoneColor,扩展属性, 双色图标主题色,注意：仅适用双色图标主题
    /// </summary>
    public AntDesignColor TwotoneColorType { get; set; }
    /// <summary>
    /// nzTwotoneColor,双色图标主题色,注意：仅适用双色图标主题
    /// </summary>
    public string TwotoneColor { get; set; }
    /// <summary>
    /// [nzTwotoneColor],双色图标主题色,注意：仅适用双色图标主题
    /// </summary>
    public string BindTwotoneColor { get; set; }
    /// <summary>
    /// nzIconfont,Iconfont图标
    /// </summary>
    public string IconFont { get; set; }
    /// <summary>
    /// [nzIconfont],Iconfont图标
    /// </summary>
    public string BindIconFont { get; set; }
    /// <summary>
    /// (click),单击事件
    /// </summary>
    public string OnClick { get; set; }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        var config = new Config( context, output, content );
        return new IconRender( config );
    }
}