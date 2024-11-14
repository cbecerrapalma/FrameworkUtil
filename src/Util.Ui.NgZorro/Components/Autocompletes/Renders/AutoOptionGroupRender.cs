﻿using Util.Ui.Builders;
using Util.Ui.NgZorro.Components.Autocompletes.Builders;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Autocompletes.Renders; 

/// <summary>
/// 自动完成选项组渲染器
/// </summary>
public class AutoOptionGroupRender : RenderBase {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;

    /// <summary>
    /// 初始化自动完成选项组渲染器
    /// </summary>
    /// <param name="config">配置</param>
    public AutoOptionGroupRender( Config config ) {
        _config = config;
    }

    /// <summary>
    /// 获取标签生成器
    /// </summary>
    protected override TagBuilder GetTagBuilder() {
        var builder = new AutoOptionGroupBuilder( _config );
        builder.Config();
        return builder;
    }

    /// <inheritdoc />
    public override IHtmlContent Clone() {
        return new AutoOptionGroupRender( _config.Copy() );
    }
}