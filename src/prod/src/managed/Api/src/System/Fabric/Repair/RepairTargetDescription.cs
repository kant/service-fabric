// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace System.Fabric.Repair
{
    using System;
    using System.Fabric.Interop;

    /// <summary>
    /// <para>Describes the entities targeted by a repair action.</para>
    /// <para>This class supports the Service Fabric platform; it is not meant to be called directly from your code.</para>
    /// </summary>
    public class RepairTargetDescription
    {
        internal RepairTargetDescription(RepairTargetKind kind)
        {
            this.Kind = kind;
        }

        /// <summary>
        /// <para>Gets the kind of the <see cref="System.Fabric.Repair.RepairTargetDescription" /> represented by the current object.</para>
        /// </summary>
        /// <value>
        /// <para>The kind of the <see cref="System.Fabric.Repair.RepairTargetDescription" /> represented by the current object.</para>
        /// </value>
        public RepairTargetKind Kind { get; private set; }

        internal static unsafe RepairTargetDescription CreateFromNative(IntPtr nativeDescriptionPtr)
        {
            if (nativeDescriptionPtr == IntPtr.Zero)
            {
                return null;
            }

            var nativeDescription = (NativeTypes.FABRIC_REPAIR_TARGET_DESCRIPTION*)nativeDescriptionPtr;
            switch (nativeDescription->Kind)
            {
                case NativeTypes.FABRIC_REPAIR_TARGET_KIND.FABRIC_REPAIR_TARGET_KIND_NODE:
                    return NodeRepairTargetDescription.CreateFromNative(nativeDescription->Value);
                default:
                    // Unknown kind; client is probably an old version. Return an empty object
                    // to indicate that target has been set but is not readable by this client.
                    return new RepairTargetDescription(RepairTargetKind.Invalid);
            }
        }

        internal virtual IntPtr ToNativeValue(PinCollection pinCollection)
        {
            return IntPtr.Zero;
        }

        internal IntPtr ToNative(PinCollection pinCollection)
        {
            var nativeDescription = new NativeTypes.FABRIC_REPAIR_TARGET_DESCRIPTION()
            {
                Kind = (NativeTypes.FABRIC_REPAIR_TARGET_KIND)this.Kind,
                Value = this.ToNativeValue(pinCollection),
            };

            return pinCollection.AddBlittable(nativeDescription);
        }
    }
}