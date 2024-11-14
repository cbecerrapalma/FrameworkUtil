﻿using Util.Ui.Angular.Configs;
using Util.Ui.Builders;
using Util.Ui.NgZorro.Components.Tags.Builders;
using Util.Ui.NgZorro.Configs;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Tags.Renders;

/// <summary>
/// 标签渲染器
/// </summary>
public class TagRender : RenderBase {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;

    /// <summary>
    /// 初始化标签渲染器
    /// </summary>
    /// <param name="config">配置</param>
    public TagRender( Config config ) {
        _config = config;
    }

    /// <summary>
    /// 获取标签生成器
    /// </summary>
    protected override TagBuilder GetTagBuilder() {
        var config = _config.Copy();
        config.RemoveAttribute( UiConst.Id );
        var containerTagBuilder = new TagContainerTagBuilder( config, _config.GetValue( UiConst.Id ) );
        containerTagBuilder.Config();
        TagBuilder container = IsEnableExtend() ? containerTagBuilder : new EmptyContainerTagBuilder();
        if (IsEnableExtend()) {
            container.AppendContent($"@for(item of {containerTagBuilder.ExtendId}.options;track item.text)");
            container.AppendContent( "{" );
        }
        var builder = new TagTagBuilder( _config, IsEnableExtend(), containerTagBuilder.ExtendId );
        container.AppendContent( builder );
        builder.Config();
        if ( IsEnableExtend() ) {
            container.AppendContent( "}" );
        }
        return container;
    }

    /// <summary>
    /// 是否启用基础扩展
    /// </summary>
    public bool IsEnableExtend() {
        if ( GetEnableExtend() == false )
            return false;
        return GetEnableExtend() == true ||
               GetUrl().IsEmpty() == false ||
               GetBindUrl().IsEmpty() == false ||
               GetData().IsEmpty() == false;
    }

    /// <summary>
    /// 获取启用扩展属性
    /// </summary>
    private bool? GetEnableExtend() {
        return _config.GetValue<bool?>( UiConst.EnableExtend );
    }

    /// <summary>
    /// 获取地址
    /// </summary>
    private string GetUrl() {
        return _config.GetValue( UiConst.Url );
    }

    /// <summary>
    /// 获取地址
    /// </summary>
    private string GetBindUrl() {
        return _config.GetValue( AngularConst.BindUrl );
    }

    /// <summary>
    /// 获取数据源
    /// </summary>
    private string GetData() {
        return _config.GetValue( UiConst.Data );
    }

    /// <inheritdoc />
    public override IHtmlContent Clone() {
        return new TagRender( _config.Copy() );
    }
}