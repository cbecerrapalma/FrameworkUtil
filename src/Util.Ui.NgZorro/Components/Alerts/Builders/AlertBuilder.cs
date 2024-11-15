﻿using Util.Ui.Angular.Builders;
using Util.Ui.Angular.Configs;
using Util.Ui.NgZorro.Enums;

namespace Util.Ui.NgZorro.Components.Alerts.Builders;

/// <summary>
/// 警告提示标签生成器
/// </summary>
public class AlertBuilder : AngularTagBuilder {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;

    /// <summary>
    /// 初始化警告提示标签生成器
    /// </summary>
    public AlertBuilder( Config config ) : base( config, "nz-alert" ) {
        _config = config;
    }

    /// <summary>
    /// 配置是否顶部公告
    /// </summary>
    public AlertBuilder Banner() {
        AttributeIfNotEmpty( "[nzBanner]", _config.GetValue( UiConst.Banner ) );
        return this;
    }

    /// <summary>
    /// 配置是否可关闭
    /// </summary>
    public AlertBuilder Closeable() {
        AttributeIfNotEmpty( "[nzCloseable]", _config.GetValue( UiConst.Closeable ) );
        return this;
    }

    /// <summary>
    /// 配置关闭按钮文字
    /// </summary>
    public AlertBuilder CloseText() {
        AttributeIfNotEmpty( "nzCloseText", _config.GetValue( UiConst.CloseText ) );
        AttributeIfNotEmpty( "[nzCloseText]", _config.GetValue( AngularConst.BindCloseText ) );
        return this;
    }

    /// <summary>
    /// 配置描述
    /// </summary>
    public AlertBuilder Description() {
        AttributeIfNotEmpty( "nzDescription", _config.GetValue( UiConst.Description ) );
        AttributeIfNotEmpty( "[nzDescription]", _config.GetValue( AngularConst.BindDescription ) );
        return this;
    }

    /// <summary>
    /// 配置提示内容
    /// </summary>
    public AlertBuilder Message() {
        AttributeIfNotEmpty( "nzMessage", _config.GetValue( UiConst.Message ) );
        BindMessage( _config.GetValue( AngularConst.BindMessage ) );
        return this;
    }

    /// <summary>
    /// 配置提示内容
    /// </summary>
    public AlertBuilder BindMessage( string value ) {
        AttributeIfNotEmpty( "[nzMessage]", value );
        return this;
    }

    /// <summary>
    /// 配置是否显示图标
    /// </summary>
    public AlertBuilder ShowIcon() {
        if ( _config.GetValue( UiConst.IconType ).IsEmpty() == false ||
            _config.GetValue( AngularConst.BindIconType ).IsEmpty() == false ||
            _config.GetValue( UiConst.Icon ).IsEmpty() == false ) {
            Attribute( "[nzShowIcon]", "true" );
            return this;
        }
        AttributeIfNotEmpty( "[nzShowIcon]", _config.GetValue( UiConst.ShowIcon ) );
        return this;
    }

    /// <summary>
    /// 配置图标
    /// </summary>
    public AlertBuilder Icon() {
        AttributeIfNotEmpty( "nzIconType", _config.GetValue<AntDesignIcon?>( UiConst.IconType )?.Description() );
        AttributeIfNotEmpty( "[nzIconType]", _config.GetValue( AngularConst.BindIconType ) );
        AttributeIfNotEmpty( "[nzIcon]", _config.GetValue( UiConst.Icon ) );
        return this;
    }

    /// <summary>
    /// 配置警告类型
    /// </summary>
    public AlertBuilder Type() {
        AttributeIfNotEmpty( "nzType", _config.GetValue<AlertType?>( UiConst.Type )?.Description() );
        AttributeIfNotEmpty( "[nzType]", _config.GetValue( AngularConst.BindType ) );
        return this;
    }

    /// <summary>
    /// 配置自定义操作项
    /// </summary>
    public AlertBuilder Action() {
        AttributeIfNotEmpty( "[nzAction]", _config.GetValue( UiConst.Action ) );
        return this;
    }

    /// <summary>
    /// 配置事件
    /// </summary>
    public AlertBuilder Events() {
        AttributeIfNotEmpty( "(nzOnClose)", _config.GetValue( UiConst.OnClose ) );
        return this;
    }

    /// <summary>
    /// 配置
    /// </summary>
    public override void Config() {
        base.ConfigBase( _config );
        Banner().Closeable().CloseText().Description()
            .Message().ShowIcon().Icon().Type().Action()
            .Events();
    }
}