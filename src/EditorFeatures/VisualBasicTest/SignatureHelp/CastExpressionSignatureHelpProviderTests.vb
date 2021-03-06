' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis.Editor.UnitTests.SignatureHelp
Imports Microsoft.CodeAnalysis.Editor.VisualBasic.SignatureHelp

Namespace Microsoft.CodeAnalysis.Editor.VisualBasic.UnitTests.SignatureHelp
    Public Class CastExpressionSignatureHelpProviderTests
        Inherits AbstractVisualBasicSignatureHelpProviderTests

        Friend Overrides Function CreateSignatureHelpProvider() As ISignatureHelpProvider
            Return New CastExpressionSignatureHelpProvider()
        End Function

        <Fact, Trait(Traits.Feature, Traits.Features.SignatureHelp)>
        Public Sub TestInvocationForCType()
            Dim markup = <a><![CDATA[
Class C
    Sub Foo()
        Dim x = CType($$
    End Sub
End Class
]]></a>.Value

            Dim expectedOrderedItems = New List(Of SignatureHelpTestItem)()
            expectedOrderedItems.Add(New SignatureHelpTestItem(
                                     $"CType({Expression1}, {VBWorkspaceResources.Typename}) As {Result}",
                                     ReturnsConvertResult,
                                     ExpressionToConvert,
                                     currentParameterIndex:=0))

            Test(markup, expectedOrderedItems)
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.SignatureHelp)>
        Public Sub TestInvocationForCTypeAfterComma()
            Dim markup = <a><![CDATA[
Class C
    Sub Foo()
        Dim x = CType(bar, $$
    End Sub
End Class
]]></a>.Value

            Dim expectedOrderedItems = New List(Of SignatureHelpTestItem)()
            expectedOrderedItems.Add(New SignatureHelpTestItem(
                                     $"CType({Expression1}, {VBWorkspaceResources.Typename}) As {Result}",
                                     ReturnsConvertResult,
                                     NameOfTypeToConvert,
                                     currentParameterIndex:=1))

            Test(markup, expectedOrderedItems)
            Test(markup, expectedOrderedItems, usePreviousCharAsTrigger:=True)
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.SignatureHelp)>
        Public Sub TestInvocationForDirectCast()
            Dim markup = <a><![CDATA[
Class C
    Sub Foo()
        Dim x = DirectCast($$
    End Sub
End Class
]]></a>.Value

            Dim expectedOrderedItems = New List(Of SignatureHelpTestItem)()
            expectedOrderedItems.Add(New SignatureHelpTestItem(
                                     $"DirectCast({Expression1}, {VBWorkspaceResources.Typename}) As {Result}",
                                     IntroducesTypeConversion,
                                     ExpressionToConvert,
                                     currentParameterIndex:=0))

            Test(markup, expectedOrderedItems)
        End Sub

        <WorkItem(530132)>
        <Fact, Trait(Traits.Feature, Traits.Features.SignatureHelp)>
        Public Sub TestInvocationForTryCast()
            Dim markup = <a><![CDATA[
Class C
    Sub Foo()
        Dim x = [|TryCast($$
    |]End Sub
End Class
]]></a>.Value

            Dim expectedOrderedItems = New List(Of SignatureHelpTestItem)()
            expectedOrderedItems.Add(New SignatureHelpTestItem(
                                     $"TryCast({Expression1}, {VBWorkspaceResources.Typename}) As {Result}",
                                     IntroducesSafeTypeConversion,
                                     ExpressionToConvert,
                                     currentParameterIndex:=0))

            Test(markup, expectedOrderedItems)
        End Sub

    End Class
End Namespace
