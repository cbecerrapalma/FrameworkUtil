﻿using Util.Ui.Angular.Configs;
using Util.Ui.NgZorro.Components.Base;
using Util.Ui.NgZorro.Components.Trees.Helpers;
using Util.Ui.NgZorro.Enums;

namespace Util.Ui.NgZorro.Components.TreeSelects.Builders;

/// <summary>
/// 树形选择标签生成器
/// </summary>
public class TreeSelectBuilder : FormControlBuilderBase<TreeSelectBuilder> {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;
    /// <summary>
    /// 树形服务
    /// </summary>
    private readonly TreeService _service;

    /// <summary>
    /// 初始化树选择标签生成器
    /// </summary>
    /// <param name="config">配置</param>
    public TreeSelectBuilder( Config config ) : base( config, "nz-tree-select" ) {
        _config = config;
        _service = new TreeService( _config );
    }

    /// <summary>
    /// 扩展标识
    /// </summary>
    private string ExtendId => _service.ExtendId;

    /// <summary>
    /// 配置输入框标识
    /// </summary>
    public TreeSelectBuilder NzId() {
        AttributeIfNotEmpty( "nzId", _config.GetValue( UiConst.NzId ) );
        AttributeIfNotEmpty( "[nzId]", _config.GetValue( AngularConst.BindNzId ) );
        return this;
    }

    /// <summary>
    /// 配置允许清除
    /// </summary>
    public TreeSelectBuilder AllowClear() {
        AttributeIfNotEmpty( "[nzAllowClear]", _config.GetValue( UiConst.AllowClear ) );
        return this;
    }

    /// <summary>
    /// 配置占位提示
    /// </summary>
    public TreeSelectBuilder Placeholder() {
        AttributeIfNotEmpty( "nzPlaceHolder", _config.GetValue( UiConst.Placeholder ) );
        AttributeIfNotEmpty( "[nzPlaceHolder]", _config.GetValue( AngularConst.BindPlaceholder ) );
        return this;
    }

    /// <summary>
    /// 配置禁用
    /// </summary>
    public TreeSelectBuilder Disabled() {
        AttributeIfNotEmpty( "[nzDisabled]", _config.GetValue( UiConst.Disabled ) );
        return this;
    }

    /// <summary>
    /// 配置是否显示图标
    /// </summary>
    public TreeSelectBuilder ShowIcon() {
        AttributeIfNotEmpty( "[nzShowIcon]", _config.GetValue( UiConst.ShowIcon ) );
        return this;
    }

    /// <summary>
    /// 配置显示搜索
    /// </summary>
    public TreeSelectBuilder ShowSearch() {
        AttributeIfNotEmpty( "[nzShowSearch]", _config.GetValue( UiConst.ShowSearch ) );
        return this;
    }

    /// <summary>
    /// 配置空列表默认内容
    /// </summary>
    public TreeSelectBuilder NotFoundContent() {
        AttributeIfNotEmpty( "nzNotFoundContent", _config.GetValue( UiConst.NotFoundContent ) );
        AttributeIfNotEmpty( "[nzNotFoundContent]", _config.GetValue( AngularConst.BindNotFoundContent ) );
        return this;
    }

    /// <summary>
    /// 配置下拉菜单和选择器同宽
    /// </summary>
    public TreeSelectBuilder DropdownMatchSelectWidth() {
        AttributeIfNotEmpty( "[nzDropdownMatchSelectWidth]", _config.GetValue( UiConst.DropdownMatchSelectWidth ) );
        return this;
    }

    /// <summary>
    /// 配置下拉菜单样式
    /// </summary>
    public TreeSelectBuilder DropdownStyle() {
        AttributeIfNotEmpty( "[nzDropdownStyle]", _config.GetValue( UiConst.DropdownStyle ) );
        return this;
    }

    /// <summary>
    /// 配置下拉菜单样式类名
    /// </summary>
    public TreeSelectBuilder DropdownClassName() {
        AttributeIfNotEmpty( "nzDropdownClassName", _config.GetValue( UiConst.DropdownClassName ) );
        AttributeIfNotEmpty( "[nzDropdownClassName]", _config.GetValue( AngularConst.BindDropdownClassName ) );
        return this;
    }

    /// <summary>
    /// 配置是否支持多选
    /// </summary>
    public TreeSelectBuilder Multiple() {
        AttributeIfNotEmpty( "[nzMultiple]", _config.GetValue( UiConst.Multiple ) );
        return this;
    }

    /// <summary>
    /// 配置是否搜索隐藏未匹配节点
    /// </summary>
    public TreeSelectBuilder HideUnmatched() {
        AttributeIfNotEmpty( "[nzHideUnMatched]", _config.GetValue( UiConst.HideUnmatched ) );
        return this;
    }

    /// <summary>
    /// 配置选择框大小
    /// </summary>
    public TreeSelectBuilder Size() {
        AttributeIfNotEmpty( "nzSize", _config.GetValue<InputSize?>( UiConst.Size )?.Description() );
        AttributeIfNotEmpty( "[nzSize]", _config.GetValue( AngularConst.BindSize ) );
        return this;
    }

    /// <summary>
    /// 配置选择框弹出位置
    /// </summary>
    public TreeSelectBuilder Placement() {
        AttributeIfNotEmpty( "nzPlacement", _config.GetValue<TreeSelectPlacement?>( UiConst.Placement )?.Description() );
        AttributeIfNotEmpty( "[nzPlacement]", _config.GetValue( AngularConst.BindPlacement ) );
        return this;
    }

    /// <summary>
    /// 配置节点前是否添加复选框
    /// </summary>
    public TreeSelectBuilder Checkable() {
        Checkable( _config.GetValue( UiConst.Checkable ) );
        return this;
    }

    /// <summary>
    /// 配置节点前是否添加复选框
    /// </summary>
    public TreeSelectBuilder Checkable( string value ) {
        AttributeIfNotEmpty( "[nzCheckable]", value );
        return this;
    }

    /// <summary>
    /// 配置严格勾选
    /// </summary>
    public TreeSelectBuilder CheckStrictly() {
        AttributeIfNotEmpty( "[nzCheckStrictly]", _config.GetValue( UiConst.CheckStrictly ) );
        return this;
    }

    /// <summary>
    /// 配置是否显示展开图标
    /// </summary>
    public TreeSelectBuilder ShowExpand() {
        AttributeIfNotEmpty( "[nzShowExpand]", _config.GetValue( UiConst.ShowExpand ) );
        return this;
    }

    /// <summary>
    /// 配置是否显示连接线
    /// </summary>
    public TreeSelectBuilder ShowLine() {
        AttributeIfNotEmpty( "[nzShowLine]", _config.GetValue( UiConst.ShowLine ) );
        return this;
    }

    /// <summary>
    /// 配置是否异步加载
    /// </summary>
    public TreeSelectBuilder AsyncData() {
        AttributeIfNotEmpty( "[nzAsyncData]", _config.GetValue( UiConst.AsyncData ) );
        return this;
    }

    /// <summary>
    /// 配置节点数据
    /// </summary>
    public TreeSelectBuilder Nodes() {
        Nodes( _config.GetValue( UiConst.Nodes ) );
        return this;
    }

    /// <summary>
    /// 配置节点数据
    /// </summary>
    public TreeSelectBuilder Nodes( string value ) {
        AttributeIfNotEmpty( "[nzNodes]", value );
        return this;
    }

    /// <summary>
    /// 配置是否默认展开所有节点
    /// </summary>
    public TreeSelectBuilder DefaultExpandAll() {
        AttributeIfNotEmpty( "[nzDefaultExpandAll]", _config.GetValue( UiConst.ExpandAll ) );
        return this;
    }

    /// <summary>
    /// 配置默认展开节点的键集合
    /// </summary>
    public TreeSelectBuilder ExpandedKeys() {
        AttributeIfNotEmpty( "[nzExpandedKeys]", _config.GetValue( UiConst.ExpandedKeys ) );
        return this;
    }

    /// <summary>
    /// 配置显示函数
    /// </summary>
    public TreeSelectBuilder DisplayWith() {
        AttributeIfNotEmpty( "[nzDisplayWith]", _config.GetValue( UiConst.DisplayWith ) );
        return this;
    }

    /// <summary>
    /// 配置最大标签数量
    /// </summary>
    public TreeSelectBuilder MaxTagCount() {
        AttributeIfNotEmpty( "[nzMaxTagCount]", _config.GetValue( UiConst.MaxTagCount ) );
        return this;
    }

    /// <summary>
    /// 配置最大标签占位符
    /// </summary>
    public TreeSelectBuilder MaxTagPlaceholder() {
        AttributeIfNotEmpty( "[nzMaxTagPlaceholder]", _config.GetValue( UiConst.MaxTagPlaceholder ) );
        return this;
    }

    /// <summary>
    /// 配置自定义节点模板
    /// </summary>
    public TreeSelectBuilder TreeTemplate() {
        AttributeIfNotEmpty( "[nzTreeTemplate]", _config.GetValue( UiConst.TreeTemplate ) );
        return this;
    }

    /// <summary>
    /// 配置虚拟高度
    /// </summary>
    public TreeSelectBuilder VirtualHeight() {
        AttributeIfNotEmpty( "nzVirtualHeight", _config.GetValue( UiConst.VirtualHeight ) );
        AttributeIfNotEmpty( "[nzVirtualHeight]", _config.GetValue( AngularConst.BindVirtualHeight ) );
        return this;
    }

    /// <summary>
    /// 配置虚拟滚动列高
    /// </summary>
    public TreeSelectBuilder VirtualItemSize() {
        AttributeIfNotEmpty( "[nzVirtualItemSize]", _config.GetValue( UiConst.VirtualItemSize ) );
        return this;
    }

    /// <summary>
    /// 配置虚拟滚动缓冲区最大高度
    /// </summary>
    public TreeSelectBuilder VirtualMaxBufferPx() {
        AttributeIfNotEmpty( "[nzVirtualMaxBufferPx]", _config.GetValue( UiConst.VirtualMaxBufferPx ) );
        return this;
    }

    /// <summary>
    /// 配置虚拟滚动缓冲区最小高度
    /// </summary>
    public TreeSelectBuilder VirtualMinBufferPx() {
        AttributeIfNotEmpty( "[nzVirtualMinBufferPx]", _config.GetValue( UiConst.VirtualMinBufferPx ) );
        return this;
    }

    /// <summary>
    /// 配置宽度
    /// </summary>
    public TreeSelectBuilder Width() {
        var style = _config.GetValue( UiConst.Style );
        if ( style.Contains( "width" ) )
            return this;
        var width = _config.GetValue( UiConst.Width );
        if ( width.IsEmpty() )
            return this;
        if ( Util.Helpers.Validation.IsNumber( width ) )
            width = $"{width}px";
        if ( style.IsEmpty() == false )
            style += ";";
        style = $"{style}width:{width}";
        _config.SetAttribute( UiConst.Style, style );
        return this;
    }

    /// <summary>
    /// 配置校验状态
    /// </summary>
    public TreeSelectBuilder Status() {
        AttributeIfNotEmpty( "nzStatus", _config.GetValue<FormControlStatus?>( UiConst.Status )?.Description() );
        AttributeIfNotEmpty( "[nzStatus]", _config.GetValue( AngularConst.BindStatus ) );
        return this;
    }

    /// <summary>
    /// 配置浮层是否应带有背景板
    /// </summary>
    public TreeSelectBuilder Backdrop() {
        AttributeIfNotEmpty( "[nzBackdrop]", _config.GetValue( UiConst.Backdrop ) );
        return this;
    }

    /// <summary>
    /// 配置事件
    /// </summary>
    public TreeSelectBuilder Events() {
        OnExpandChange( _config.GetValue( UiConst.OnExpandChange ) );
        return this;
    }

    /// <summary>
    /// 展开收缩节点事件
    /// </summary>
    public TreeSelectBuilder OnExpandChange( string value ) {
        AttributeIfNotEmpty( "(nzExpandChange)", value );
        return this;
    }

    /// <summary>
    /// 配置
    /// </summary>
    public override void Config() {
        ConfigForm().NzId().Name().AllowClear().Placeholder().Disabled().ShowIcon()
            .ShowSearch().NotFoundContent().DropdownMatchSelectWidth()
            .DropdownStyle().DropdownClassName().Multiple()
            .HideUnmatched().Size().Placement().Checkable().CheckStrictly()
            .ShowExpand().ShowLine().AsyncData().Nodes()
            .DefaultExpandAll().ExpandedKeys().DisplayWith()
            .MaxTagCount().MaxTagPlaceholder().TreeTemplate()
            .VirtualHeight().VirtualItemSize().VirtualMaxBufferPx().VirtualMinBufferPx()
            .Width().Status().Backdrop()
            .Events();
        base.ConfigBase( _config );
        _service.ConfigBuilder( this );
        ConfigDefault();
    }

    /// <summary>
    /// 配置默认属性
    /// </summary>
    private void ConfigDefault() {
        if ( _service.IsEnableExtend() == false )
            return;
        Nodes( $"{ExtendId}.dataSource" )
            .LoadKeys()
            .OnExpandChange( $"{ExtendId}.expandChange($event)" );
    }

    /// <summary>
    /// 配置加载标识列表
    /// </summary>
    public TreeSelectBuilder LoadKeys() {
        AttributeIfNotEmpty( "[loadKeys]", _config.GetValue( UiConst.LoadKeys ) );
        return this;
    }
}