﻿using Util.Ui.Builders;
using Util.Ui.NgZorro.Components.Anchors.Builders;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Anchors.Renders; 

/// <summary>
/// 锚点渲染器
/// </summary>
public class AnchorRender : RenderBase {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;

    /// <summary>
    /// 初始化锚点渲染器
    /// </summary>
    /// <param name="config">配置</param>
    public AnchorRender( Config config ) {
        _config = config;
    }

    /// <summary>
    /// 获取标签生成器
    /// </summary>
    protected override TagBuilder GetTagBuilder() {
        var builder = new AnchorBuilder( _config );
        builder.Config();
        return builder;
    }

    /// <inheritdoc />
    public override IHtmlContent Clone() {
        return new AnchorRender( _config.Copy() );
    }
}