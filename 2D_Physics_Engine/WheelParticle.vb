Option Strict Off
Public Class WheelParticle
    Inherits CircleParticle
    Private rp As RimParticle
    Private tan As Vector
    Private normSlip As Vector
    Private _edgePositions As New ArrayList()
    Private _edgeParticles As New ArrayList()
    Private _traction As Double
    Public Sub New(x As Double, y As Double, radius As Double, fixed As Boolean, mass As Double, elasticity As Double, _
     friction As Double, traction As Double)
        MyBase.new(x, y, radius, fixed, mass, elasticity, _
         friction)
        tan = New Vector(0, 0)
        normSlip = New Vector(0, 0)
        rp = New RimParticle(radius, 2)
        setTraction(traction)
        _edgePositions = getEdgePositions()
        _edgeParticles = getEdgeParticles()
    End Sub
    Public Function getAngularVelocity() As Double
        Return rp.getAngularVelocity()
    End Function
    Public Sub setAngularVelocity(a As Double)
        rp.setAngularVelocity(a)
    End Sub
    Public Function getTraction() As Double
        Return 1 - _traction
    End Function
    Public Sub setTraction(t As Double)
        _traction = 1 - t
    End Sub
    Public Function getEdgeParticles() As ArrayList
        If _edgePositions.Count() = 0 Then
            getEdgePositions()
        End If
        If _edgeParticles.Count() = 0 Then
            Dim cp1 As New CircleParticle(0, 0, 1, False, 1, 0.3, _
             0)
            cp1.setCollidable(False)
            cp1.setVisible(False)
            APEngine.addParticle(cp1)
            Dim cp2 As New CircleParticle(0, 0, 1, False, 1, 0.3, _
             0)
            cp2.setCollidable(False)
            cp2.setVisible(False)
            APEngine.addParticle(cp2)
            Dim cp3 As New CircleParticle(0, 0, 1, False, 1, 0.3, _
             0)
            cp3.setCollidable(False)
            cp3.setVisible(False)
            APEngine.addParticle(cp3)
            Dim cp4 As New CircleParticle(0, 0, 1, False, 1, 0.3, _
             0)
            cp4.setCollidable(False)
            cp4.setVisible(False)
            APEngine.addParticle(cp4)
            _edgeParticles.Add(cp1)
            _edgeParticles.Add(cp2)
            _edgeParticles.Add(cp3)
            _edgeParticles.Add(cp4)
            updateEdgeParticles()
        End If
        Return _edgeParticles
    End Function
    Public Function getEdgePositions() As ArrayList
        If _edgePositions.Count() = 0 Then
            _edgePositions.Add(New Vector(0, 0))
            _edgePositions.Add(New Vector(0, 0))
            _edgePositions.Add(New Vector(0, 0))
            _edgePositions.Add(New Vector(0, 0))
            updateEdgePositions()
        End If
        Return _edgePositions
    End Function
    Public Overrides Sub paint()
        Dim px As Single = curr.x
        Dim py As Single = CDbl(curr.y)
        Dim rx As Single = rp.curr.x
        Dim ry As Single = rp.curr.y
        If dc Is Nothing Then
            dc = getDefaultContainer()
        End If
        If Not getVisible() Then
            Return
        End If
        'Dim f1 As New GeneralPath()
        'Dim f As path
        ' f1.moveTo(px, py)
        ' f1.lineTo(rx + px, ry + py)
        ' 'f1.moveTo(px, py)
        'f1.lineTo(-rx + px, -ry + py)
        'f1.moveTo(px, py)
        'f1.lineTo(-ry + px, rx + py)
        'f1.moveTo(px, py)
        'f1.lineTo(ry + px, -rx + py)
        'dc.draw(f1)
        dc.DrawLine(New Pen(Color.Black), New Point(px, py), New Point(rx + px, ry + py))
        dc.DrawLine(New Pen(Color.Black), New Point(px, py), New Point(-rx + px, -ry + py))
        dc.DrawLine(New Pen(Color.Black), New Point(px, py), New Point(-ry + px, rx + py))
        dc.DrawLine(New Pen(Color.Black), New Point(px, py), New Point(ry + px, -rx + py))
        'Dim circle As New Ellipse2D.Double(curr.x - getRadius(), curr.y - getRadius(), DirectCast(getRadius(), Double) * 2, DirectCast(getRadius(), Double) * 2)
        'dc.draw(circle)
        dc.DrawEllipse(New Pen(Color.Black), CSng(curr.x - getRadius()), CSng(curr.y - getRadius()), CSng(getRadius()) * 2, CSng(getRadius()) * 2)
       

    End Sub
    Public Overrides Sub update(dt As Double)
        MyBase.update(dt)
        rp.update(dt)
        If Not _edgePositions Is Nothing Then
            updateEdgePositions()
        End If
        If Not _edgeParticles Is Nothing Then
            updateEdgeParticles()
        End If
    End Sub
    Public Overrides Sub resolveCollision(mtd As Vector, velocity As Vector, normal As Vector, depth As Double, order As Double)
        MyBase.resolveCollision(mtd, velocity, normal, depth, order)
        resolve(normal.mult(sign(depth * order)))
    End Sub
    Private Sub resolve(n As Vector)
        tan.setTo(-rp.curr.y, rp.curr.x)
        tan = tan.normalize()
        'velocity of the wheel's surface 
        Dim wheelSurfaceVelocity As Vector = tan.mult(rp.getSpeed())
        ' the velocity of the wheel's surface relative to the ground
        Dim combinedVelocity As Vector = getVelocity().plusEquals(wheelSurfaceVelocity)
        'the wheel's comb velocity projected onto the contact normal
        Dim cp As Double = combinedVelocity.cross(n)
        'set the wheel's spinspeed to track the ground
        tan.multEquals(cp)
        rp.prev.copy(rp.curr.minus(tan))
        ' some of the wheel's torque is removed and converted into linear displacement
        Dim slipSpeed As Double = (1 - _traction) * rp.getSpeed()
        normSlip.setTo(slipSpeed * n.y, slipSpeed * n.x)
        curr.plusEquals(normSlip)
        rp.setSpeed(rp.getSpeed() * _traction)
    End Sub
    Private Sub updateEdgePositions()
        Dim px As Double = curr.x
        Dim py As Double = curr.y
        Dim rx As Double = rp.curr.x
        Dim ry As Double = rp.curr.y
        DirectCast(_edgePositions.Item(0), Vector).setTo(rx + px, ry + py)
        DirectCast(_edgePositions.Item(1), Vector).setTo(-ry + px, rx + py)
        DirectCast(_edgePositions.Item(2), Vector).setTo(-rx + px, -ry + py)
        DirectCast(_edgePositions.Item(3), Vector).setTo(ry + px, -rx + py)
    End Sub
    Private Sub updateEdgeParticles()
        Dim i As Integer = 0
        For i = 0 To 3
            _edgeParticles.Item(i).setpx((DirectCast(_edgePositions.Item(i), Vector)).x)
            _edgeParticles.Item(i).setpy((DirectCast(_edgePositions.Item(i), Vector)).y)
            ' System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        Next
    End Sub
    Private Function sign(val As Double) As Integer
        If val < 0 Then
            Return -1
        End If
        Return 1
    End Function
End Class


