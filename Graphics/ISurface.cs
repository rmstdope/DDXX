using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface ISurface : IDisposable
    {
        Surface DXSurface { get; }
        // Summary:
        //     Retrieves the description of a surface.
        SurfaceDescription Description { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        bool Disposed { get; }
        //
        // Summary:
        //     Retrieves a device context.
        //
        // Returns:
        //     A System.Drawing.Graphics object that represents the device context for the
        //     surface.
        System.Drawing.Graphics GetGraphics();
        //
        // Summary:
        //     Locks a rectangle on a surface.
        //
        // Parameters:
        //   flags:
        //     A Direct3D.LockFlags object that specifies the type of
        //     lock to apply.
        //
        // Returns:
        //     A GraphicsStream object that describes the locked region.
        GraphicsStream LockRectangle(LockFlags flags);
        //
        // Summary:
        //     Locks a rectangle on a surface.
        //
        // Parameters:
        //   flags:
        //     A Direct3D.LockFlags object that specifies the type of
        //     lock to apply.
        //
        //   pitch:
        //     Integer that represents the returned pitch value of the locked region.
        //
        // Returns:
        //     A GraphicsStream object that describes the locked region.
        GraphicsStream LockRectangle(LockFlags flags, out int pitch);
        //
        // Summary:
        //     Locks a rectangle on a surface.
        //
        // Parameters:
        //   rect:
        //     A System.Drawing.Rectangle object that represents the rectangle to lock.
        //
        //   flags:
        //     A Direct3D.LockFlags object that specifies the type of
        //     lock to apply.
        //
        // Returns:
        //     A GraphicsStream object that describes the locked region.
        GraphicsStream LockRectangle(Rectangle rect, LockFlags flags);
        //
        // Summary:
        //     Locks a rectangle on a surface.
        //
        // Parameters:
        //   rect:
        //     A System.Drawing.Rectangle object that represents the rectangle to lock.
        //
        //   flags:
        //     A Direct3D.LockFlags object that specifies the type of
        //     lock to apply.
        //
        //   pitch:
        //     Integer that represents the returned pitch value of the locked region.
        //
        // Returns:
        //     A GraphicsStream object that describes the locked region.
        GraphicsStream LockRectangle(Rectangle rect, LockFlags flags, out int pitch);
        //
        // Summary:
        //     Locks a rectangle on a surface.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that specifies the type of array to create.
        //
        //   flags:
        //     A Direct3D.LockFlags object that specifies the type of
        //     lock to apply.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returned array. The maximum number of Direct3D.Surface.LockRectangle()
        //     allowed is three.
        //
        // Returns:
        //     An System.Array that describes the locked region.
        Array LockRectangle(Type typeLock, LockFlags flags, params int[] ranks);
        //
        // Summary:
        //     Locks a rectangle on a surface.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that specifies the type of array to create.
        //
        //   flags:
        //     A Direct3D.LockFlags object that specifies the type of
        //     lock to apply.
        //
        //   pitch:
        //     Integer that represents the returned pitch value of the locked region.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returned array. The maximum number of Direct3D.Surface.LockRectangle()
        //     allowed is three.
        //
        // Returns:
        //     An System.Array that describes the locked region.
        Array LockRectangle(Type typeLock, LockFlags flags, out int pitch, params int[] ranks);
        //
        // Summary:
        //     Locks a rectangle on a surface.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that specifies the type of array to create.
        //
        //   rect:
        //     A System.Drawing.Rectangle object that represents the rectangle to lock.
        //
        //   flags:
        //     A Direct3D.LockFlags object that specifies the type of
        //     lock to apply.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returned array. The maximum number of Direct3D.Surface.LockRectangle()
        //     allowed is three.
        //
        // Returns:
        //     An System.Array that describes the locked region.
        Array LockRectangle(Type typeLock, Rectangle rect, LockFlags flags, params int[] ranks);
        //
        // Summary:
        //     Locks a rectangle on a surface.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that specifies the type of array to create.
        //
        //   rect:
        //     A System.Drawing.Rectangle object that represents the rectangle to lock.
        //
        //   flags:
        //     A Direct3D.LockFlags object that specifies the type of
        //     lock to apply.
        //
        //   pitch:
        //     Integer that represents the returned pitch value of the locked region.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returned array. The maximum number of Direct3D.Surface.LockRectangle()
        //     allowed is three.
        //
        // Returns:
        //     An System.Array that describes the locked region.
        Array LockRectangle(Type typeLock, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks);
        //
        // Summary:
        //     Releases the device context obtained by calling Direct3D.Surface.GetGraphics().
        void ReleaseGraphics();
        //
        // Summary:
        //     Unlocks a rectangle on a surface.
        void UnlockRectangle();
    }
}
