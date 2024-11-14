﻿using Util.Ui.Builders;
using Util.Ui.Configs;
using Util.Ui.NgZorro.Components.Tables.Builders;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Tables.Renders; 

/// <summary>
/// 表头单元格过滤器触发按钮渲染器
/// </summary>
public class FilterTriggerRender : RenderBase {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;

    /// <summary>
    /// 初始化表头单元格过滤器触发按钮渲染器
    /// </summary>
    /// <param name="config">配置</param>
    public FilterTriggerRender( Config config ) {
        _config = config;
    }

    /// <summary>
    /// 获取标签生成器
    /// </summary>
    protected override TagBuilder GetTagBuilder() {
        var builder = new FilterTriggerBuilder( _config );
        builder.Config();
        return builder;
    }

    /// <inheritdoc />
    public override IHtmlContent Clone() {
        return new FilterTriggerRender( _config.Copy() );
    }
}