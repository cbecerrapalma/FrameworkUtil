﻿using Util.Ui.Builders;
using Util.Ui.NgZorro.Components.Carousels.Builders;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Carousels.Renders; 

/// <summary>
/// 走马灯内容渲染器
/// </summary>
public class CarouselContentRender : RenderBase {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;

    /// <summary>
    /// 初始化走马灯内容渲染器
    /// </summary>
    /// <param name="config">配置</param>
    public CarouselContentRender( Config config ) {
        _config = config;
    }

    /// <summary>
    /// 获取标签生成器
    /// </summary>
    protected override TagBuilder GetTagBuilder() {
        var builder = new CarouselContentBuilder( _config );
        builder.Config();
        return builder;
    }

    /// <inheritdoc />
    public override IHtmlContent Clone() {
        return new CarouselContentRender( _config.Copy() );
    }
}