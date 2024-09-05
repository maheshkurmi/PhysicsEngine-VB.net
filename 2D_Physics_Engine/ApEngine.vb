
'APE (Actionscript Physics Engine) is an AS3 open source 2D physics engine
'           Author: Alec Cove
'VB Port:   Author: Kurmi 
'           email : mahesh_kurmi2003@yahoo.com
'java Port: Author: Theo Galanakis

Option Strict Off
Public Class APEngine
    Public Const STANDARD As Integer = 100
    Public Const SELECTIVE As Integer = 200
    Public Const SIMPLE As Integer = 300
    Public Shared force As New Vector(0, 0)
    Public Shared masslessForce As New Vector(0, 0)
    Private Shared timeStep As Double
    Private Shared particles As New ArrayList()
    Private Shared constraints As New ArrayList()
    Private Shared _damping As Double
    Private Shared _defaultContainer As Graphics
    Private Shared _collisionResponseMode As Integer = STANDARD
    Public Shared bmp As New Bitmap(600, 300)
    Public Shared Sub init(dt As Double, Optional width As Integer = 700, Optional height As Integer = 350)
        timeStep = dt * dt
        _damping = 1
        bmp = New Bitmap(width, height)
        _defaultContainer = Graphics.FromImage(bmp)
        _defaultContainer.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
    End Sub
    Public Shared Function getDamping() As Double
        Return _damping
    End Function
    Public Shared Sub setDamping(d As Double)
        _damping = d
    End Sub
    Public Shared Function getDefaultContainer() As Graphics
        Return _defaultContainer
    End Function
    Public Shared Sub setDefaultContainer(s As Graphics)
        _defaultContainer = s
    End Sub
    Public Shared Function getCollisionResponseMode() As Integer
        Return _collisionResponseMode
    End Function
    Public Shared Sub setCollisionResponseMode(m As Integer)
        _collisionResponseMode = m
    End Sub
    Public Shared Sub addForce(v As Vector)
        force.plusEquals(v)
    End Sub
    Public Shared Sub addMasslessForce(v As Vector)
        masslessForce.plusEquals(v)
    End Sub
    Public Shared Sub addParticle(p As AbstractParticle)
        particles.Add(p)
    End Sub
    Public Shared Sub removeParticle(p As AbstractParticle)
        Dim ppos As Integer = particles.IndexOf(p)
        If ppos = -1 Then
            Return
        End If
        'particles.Remove(ppos)
        particles.RemoveAt(ppos)
    End Sub
    Public Shared Sub addConstraint(c As AbstractConstraint)
        constraints.Add(c)
    End Sub
    Public Shared Sub removeConstraint(c As AbstractConstraint)
        Dim cpos As Integer = constraints.IndexOf(c)
        If cpos = -1 Then
            Return
        End If
        ' constraints.Remove(cpos)
        constraints.RemoveAt(cpos)

    End Sub
    Public Shared Function getAll() As ArrayList
        Dim a As ArrayList = DirectCast(particles.Clone(), ArrayList)
        a.AddRange(constraints)
        Return a
    End Function
    Public Shared Function getAllParticles() As ArrayList
        Return particles
    End Function
    Public Shared Function getCustomParticles() As ArrayList
        Dim customParticles As New ArrayList()
        Dim i As Integer = 0
        For i = 0 To particles.Count - 1
            Dim p As AbstractParticle = DirectCast(particles.Item(i), AbstractParticle)
            If isCustomParticle(p) Then
                customParticles.Add(p)
            End If
            ' System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        Next
        Return customParticles
    End Function
    Public Shared Function getAPEParticles() As ArrayList
        Dim apeParticles As New ArrayList()
        Dim i As Integer = 0
        For i = 0 To particles.Count - 1
            Dim p As AbstractParticle = particles.Item(i)
            If Not isCustomParticle(p) Then
                apeParticles.Add(p)
            End If
            'System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        Next
        Return apeParticles
    End Function
    Public Shared Function getAllConstraints() As ArrayList
        Return constraints
    End Function
    Public Shared Sub StepUp()   '[step]()
        _defaultContainer.Clear(Color.WhiteSmoke)
        integrate()
        satisfyConstraints()
        checkCollisions()
    End Sub
    Private Shared Function isCustomParticle(p As AbstractParticle) As Boolean
        Dim isWP As Boolean = False
        Dim isCP As Boolean = False
        Dim isRP As Boolean = False
        Dim className As String = p.ToString
        If TypeOf p Is WheelParticle Then
            isWP = True
        End If
        If TypeOf p Is CircleParticle Then
            isCP = True
        End If
        If TypeOf p Is RectangleParticle Then
            isRP = True
        End If
        If Not (isWP OrElse isCP OrElse isRP) Then
            Return True
        End If
        Return False
    End Function
    Private Shared Sub integrate()
        Dim i As Integer = 0

        For i = 0 To particles.Count - 1
            If TypeOf particles.Item(i) Is RectangleParticle Then
                'DirectCast(particles.Item(i), RectangleParticle).update(timeStep)
                particles.Item(i).update(timeStep)
            ElseIf TypeOf particles.Item(i) Is CircleParticle Then
                'DirectCast(particles.Item(i), CircleParticle).update(timeStep)
                particles.Item(i).update(timeStep)
            End If
        Next

    End Sub
    Private Shared Sub satisfyConstraints()
        Dim n As Integer
        'While n < constraints.Count()
        'DirectCast(constraints.Item(n), AbstractConstraint).resolve()
        'System.Math.Max(System.Threading.Interlocked.Increment(n), n - 1)
        For n = 0 To constraints.Count - 1
            ' DirectCast(constraints.Item(n), SpringConstraint).resolve()
            constraints.Item(n).resolve()
        Next

        'End While
    End Sub
    Private Shared Sub checkCollisions()

        For j As Integer = 0 To particles.Count - 1
            Dim pa As AbstractParticle = particles.Item(j) 'DirectCast(particles.Item(j), AbstractParticle)

            Dim i As Integer

            For i = j + 1 To particles.Count - 1
                Dim pb As AbstractParticle = particles.Item(i) 'DirectCast(particles.Item(i), AbstractParticle)
                If (pa.getCollidable() And pb.getCollidable()) Then
                    CollisionDetector.test(pa, pb)
                End If
            Next

            For n As Integer = 0 To constraints.Count - 1
                If TypeOf constraints.Item(n) Is AngularConstraint Then
                    'Do nothing
                Else
                    Dim c As SpringConstraint = constraints.Item(n)
                    If (pa.getCollidable() And c.getCollidable() And (Not c.isConnectedTo(pa))) Then
                        CollisionDetector.test(pa, c.getCollisionRect())
                    End If
                End If
            Next
            pa.isColliding = False
        Next
    End Sub
End Class


