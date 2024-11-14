﻿using Util.Ui.Angular.Builders;
using Util.Ui.Angular.Configs;
using Util.Ui.Angular.Extensions;
using Util.Ui.NgZorro.Components.TreeViews.Configs;
using Util.Ui.NgZorro.Directives.Popconfirms;
using Util.Ui.NgZorro.Directives.Popover;
using Util.Ui.NgZorro.Directives.Tooltips;
using Util.Ui.NgZorro.Enums;

namespace Util.Ui.NgZorro.Components.Icons.Builders;

/// <summary>
/// 图标标签生成器
/// </summary>
public class IconBuilder : AngularTagBuilder {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;

    /// <summary>
    /// 初始化图标标签生成器
    /// </summary>
    /// <param name="config">配置</param>
    public IconBuilder( Config config ) : base( config, "span" ) {
        base.Attribute( "nz-icon" );
        _config = config;
    }

    /// <summary>
    /// 配置图标
    /// </summary>
    public IconBuilder Icon() {
        AttributeIfNotEmpty( "nzType", _config.GetValue<AntDesignIcon?>( UiConst.Icon )?.Description() );
        return this;
    }

    /// <summary>
    /// 配置图标类型
    /// </summary>
    public IconBuilder Type() {
        return Type( _config.GetValue<AntDesignIcon?>( UiConst.Type ) )
            .BindType( _config.GetValue( AngularConst.BindType ) );
    }

    /// <summary>
    /// 配置图标类型
    /// </summary>
    public IconBuilder Type( AntDesignIcon? type ) {
        return Type( type?.Description() );
    }

    /// <summary>
    /// 配置图标类型
    /// </summary>
    public IconBuilder Type( string type ) {
        AttributeIfNotEmpty( "nzType", type );
        return this;
    }

    /// <summary>
    /// 配置图标类型
    /// </summary>
    public IconBuilder BindType( string type ) {
        AttributeIfNotEmpty( "[nzType]", type );
        return this;
    }

    /// <summary>
    /// 配置图标主题
    /// </summary>
    public IconBuilder Theme() {
        return Theme( _config.GetValue<IconTheme?>( UiConst.Theme ) )
            .BindTheme( _config.GetValue( AngularConst.BindTheme ) );
    }

    /// <summary>
    /// 配置图标主题
    /// </summary>
    /// <param name="theme">主题</param>
    public IconBuilder Theme( IconTheme? theme ) {
        return Theme( theme?.Description() );
    }

    /// <summary>
    /// 配置图标主题
    /// </summary>
    /// <param name="theme">主题</param>
    public IconBuilder Theme( string theme ) {
        AttributeIfNotEmpty( "nzTheme", theme );
        return this;
    }

    /// <summary>
    /// 配置图标主题
    /// </summary>
    /// <param name="theme">主题</param>
    public IconBuilder BindTheme( string theme ) {
        AttributeIfNotEmpty( "[nzTheme]", theme );
        return this;
    }

    /// <summary>
    /// 配置双色图标主题色
    /// </summary>
    public IconBuilder Color() {
        var value = _config.GetValue<AntDesignColor?>( UiConst.TwotoneColorType )?.Description();
        if ( value.IsEmpty() == false )
            _config.SetAttribute( UiConst.TwotoneColor, value );
        if ( _config.Contains( UiConst.TwotoneColor ) || _config.Contains( AngularConst.BindTwotoneColor ) )
            Attribute( "nzTheme", IconTheme.Twotone.Description() );
        AttributeIfNotEmpty( "nzTwotoneColor", _config.GetValue( UiConst.TwotoneColor ) );
        AttributeIfNotEmpty( "[nzTwotoneColor]", _config.GetValue( AngularConst.BindTwotoneColor ) );
        return this;
    }

    /// <summary>
    /// 配置图标旋转
    /// </summary>
    public IconBuilder Spin() {
        AttributeIfNotEmpty( "[nzSpin]", _config.GetValue( UiConst.Spin ) );
        return this;
    }

    /// <summary>
    /// 配置旋转
    /// </summary>
    public IconBuilder Rotate() {
        AttributeIfNotEmpty( "[nzRotate]", _config.GetValue( UiConst.Rotate ) );
        return this;
    }

    /// <summary>
    /// 配置Iconfont图标
    /// </summary>
    public IconBuilder IconFont() {
        AttributeIfNotEmpty( "nzIconfont", _config.GetValue( UiConst.IconFont ) );
        AttributeIfNotEmpty( "[nzIconfont]", _config.GetValue( AngularConst.BindIconFont ) );
        return this;
    }

    /// <summary>
    /// 配置事件
    /// </summary>
    public IconBuilder Events() {
        this.OnClick( _config );
        return this;
    }

    /// <summary>
    /// 配置
    /// </summary>
    public override void Config() {
        base.Config();
        this.Tooltip( _config ).Popover( _config ).Popconfirm( _config );
        Type().Theme().Color().Spin().Rotate().IconFont().Events();
        AddTreeNodeToggleIcon();
    }

    /// <summary>
    /// 添加TreeNodeToggle图标指令
    /// </summary>
    private void AddTreeNodeToggleIcon() {
        var shareConfig = _config.GetValueFromItems<TreeNodeToggleShareConfig>();
        if ( shareConfig == null )
            return;
        if ( shareConfig.ActiveIcon ) {
            Attribute( "nzTreeNodeToggleActiveIcon" );
            return;
        }
        if ( shareConfig.RotateIcon )
            Attribute( "nzTreeNodeToggleRotateIcon" );
    }
}