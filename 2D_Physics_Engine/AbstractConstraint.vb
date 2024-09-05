Public Class AbstractConstraint
    Protected dc As Graphics
    Private _visible As Boolean
    Private _stiffness As Double
    Public Sub New(stiffness As Double)
        _visible = True
        _stiffness = stiffness
    End Sub
    Public Function getStiffness() As Double
        Return _stiffness
    End Function
    Public Sub setStiffness(s As Double)
        _stiffness = s
    End Sub
    Public Function getVisible() As Boolean
        Return _visible
    End Function
    Public Sub setVisible(v As Boolean)
        _visible = v
    End Sub
    Public Overridable Sub Resolve()
    End Sub
    Protected Function getDefaultContainer() As Graphics
        If APEngine.getDefaultContainer() Is Nothing Then
            Dim err As [String] = ""
            err += "You must set the defaultContainer property of the APEngine class "
            err += "if you wish to use the default paint methods of the constraints"
            Throw New Error1
        End If
        Dim parentContainer As Graphics = APEngine.getDefaultContainer()
        Return parentContainer
    End Function
End Class


