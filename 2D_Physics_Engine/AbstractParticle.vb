Public MustInherit Class AbstractParticle
    Public curr As Vector
    Public prev As Vector
    Public isColliding As Boolean
    Public interval As Interval
    Protected dc As Graphics
    Private forces As Vector
    Private temp As Vector
    Private _kfr As Double
    Private _mass As Double
    Private _invMass As Double
    Private _fixed As Boolean
    Private _visible As Boolean
    Private _friction As Double
    Private _collidable As Boolean
    Private collision As Collision
    Public Sub New(x As Double, y As Double, isFixed As Boolean, mass As Double, elasticity As Double, friction As Double)
        interval = New Interval(0, 0)
        curr = New Vector(x, y)
        prev = New Vector(x, y)
        temp = New Vector(0, 0)
        setFixed(isFixed)
        forces = New Vector(0, 0)
        collision = New Collision(New Vector(0, 0), New Vector(0, 0))
        isColliding = False
        setMass(mass)
        setElasticity(elasticity)
        setFriction(friction)
        setCollidable(True)
        setVisible(True)
    End Sub
    Public Overridable Function getMass() As Double
        Return _mass
    End Function
    Public Overridable Sub setMass(m As Double)
        If m <= 0 Then
            Throw New Error1 ']("mass may not be set <= 0")
        End If
        _mass = m
        _invMass = 1 / _mass
    End Sub
    Public Function getElasticity() As Double
        Return _kfr
    End Function
    Public Sub setElasticity(k As Double)
        _kfr = k
    End Sub
    Public Function getVisible() As Boolean
        Return _visible
    End Function
    Public Sub setVisible(v As Boolean)
        _visible = v
    End Sub
    Public Function getFriction() As Double
        Return _friction
    End Function
    Public Sub setFriction(f As Double)
        If f < 0 Or f > 1 Then
            Throw New Error1 ']("Legal friction must be >= 0 and <=1")
        End If
        _friction = f
    End Sub
    Public Function getFixed() As Boolean
        Return _fixed
    End Function
    Public Sub setFixed(f As Boolean)
        _fixed = f
    End Sub
    Public Function getPosition() As Vector
        Return New Vector(curr.x, curr.y)
    End Function
    Public Sub setPosition(p As Vector)
        curr.copy(p)
        prev.copy(p)
    End Sub
    Public Function getpx() As Double
        Return curr.x
    End Function
    Public Sub setpx(x As Double)
        curr.x = x
        prev.x = x
    End Sub
    Public Function getpy() As Double
        Return curr.y
    End Function
    Public Sub setpy(y As Double)
        curr.y = y
        prev.y = y
    End Sub
    Public Overridable Function getVelocity() As Vector

        If Math.Abs(curr.minus(prev).x) > 5 Then
            Dim i As Integer = 9
        End If
        Return curr.minus(prev)
    End Function
    Public Overridable Sub setVelocity(v As Vector)
        prev = curr.minus(v)
    End Sub
    Public Function getCollidable() As Boolean
        Return _collidable
    End Function
    Public Sub setCollidable(b As Boolean)
        _collidable = b
    End Sub
    Public Sub addForce(f As Vector)
        forces.plusEquals(f.multEquals(_invMass))
    End Sub
    Public Sub addMasslessForce(f As Vector)
        forces.plusEquals(f)
    End Sub
    Public Overridable Sub update(dt2 As Double)
        If _fixed Then
            Return
            Exit Sub
        End If
        addForce(APEngine.force)
        addMasslessForce(APEngine.masslessForce)
        temp.copy(curr)
        Dim nv As Vector = getVelocity().plus(forces.multEquals(dt2))
        curr.plusEquals(nv.multEquals(APEngine.getDamping()))
        prev.copy(temp)
        forces.setTo(0, 0)
    End Sub
    Public Function getComponents(collisionNormal As Vector) As Collision
        Dim vel As Vector = getVelocity()
        If vel.x > 5 Then
            Dim i As Integer = 9
        End If
        Dim vdotn As Double = collisionNormal.dot(vel)
        collision.vn = collisionNormal.mult(vdotn)
        collision.vt = vel.minus(collision.vn)
        

        Return collision
    End Function
    Public Overridable Sub resolveCollision(mtd As Vector, vel As Vector, n As Vector, d As Double, o As Double)
        curr.plusEquals(mtd)
        Select Case APEngine.getCollisionResponseMode()
            Case APEngine.STANDARD
                setVelocity(vel)
                '   Exit Select
            Case APEngine.SELECTIVE
                If Not isColliding Then
                    setVelocity(vel)
                End If
                isColliding = True
                ' Exit Select
            Case APEngine.SIMPLE
                ' Exit Select
        End Select
    End Sub
    Public Function getInvMass() As Double
        Return _invMass
    End Function
    Public Function getDefaultContainer() As Graphics
        If APEngine.getDefaultContainer() Is Nothing Then
            Dim err As [String] = ""
            err += "You must set the defaultContainer property of the APEngine class "
            err += "if you wish to use the default paint methods of the particles"
            Throw New Error1 '](err)
        End If
        Dim parentContainer As Graphics = APEngine.getDefaultContainer()
        Return parentContainer
    End Function
    Public Overridable Function getProjection(axis As Vector) As Interval
        Return Nothing
    End Function
End Class


