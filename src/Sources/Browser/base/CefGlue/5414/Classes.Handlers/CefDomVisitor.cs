﻿// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI


using WinFormium.CefGlue.Interop;

namespace WinFormium.CefGlue;
/// <summary>
/// Interface to implement for visiting the DOM. The methods of this class will
/// be called on the render process main thread.
/// </summary>
public abstract unsafe partial class CefDomVisitor
{
    private void visit(cef_domvisitor_t* self, cef_domdocument_t* document)
    {
        CheckSelf(self);

        var m_document = CefDomDocument.FromNative(document);

        Visit(m_document);

        m_document.Dispose();
    }

    /// <summary>
    /// Method executed for visiting the DOM. The document object passed to this
    /// method represents a snapshot of the DOM at the time this method is
    /// executed. DOM objects are only valid for the scope of this method. Do not
    /// keep references to or attempt to access any DOM objects outside the scope
    /// of this method.
    /// </summary>
    protected abstract void Visit(CefDomDocument document);
}
