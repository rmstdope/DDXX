using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dope.DDXX.Graphics
{
    public class ModelMeshCollectionAdapter
    {
        private ModelMeshCollection collection;

        public ModelMeshCollectionAdapter(ModelMeshCollection collection)
        {
            this.collection = collection;
        }

        // Summary:
        //     Gets the number of elements contained in the System.Collections.ObjectModel.ReadOnlyCollection<T>
        //     instance.
        //
        // Returns:
        //     The number of elements contained in the System.Collections.ObjectModel.ReadOnlyCollection<T>
        //     instance.
        public int Count { get { return collection.Count; } }
        // Summary:
        //     Gets the element at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the element to get.
        //
        // Returns:
        //     The element at the specified index.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than zero.-or-index is equal to or greater than System.Collections.ObjectModel.ReadOnlyCollection<T>.Count.
        public IModelMesh this[int index] { get { return new ModelMeshAdapter(collection[index]); } }
        // Summary:
        //     Retrieves a ModelMesh from the collection, given the name of the mesh.
        //
        // Parameters:
        //   meshName:
        //     The name of the mesh to retrieve.
        //
        // Returns:
        //     The ModelMesh identified by meshName.
        IModelMesh this[string meshName] { get { return new ModelMeshAdapter(collection[meshName]); } }
        // Summary:
        //     Finds a mesh with a given name if it exists in the collection.
        //
        // Parameters:
        //   meshName:
        //     The name of the mesh to find.
        //
        //   value:
        //     The mesh named meshName, if found.
        //
        // Returns:
        //     true if the named mesh is found; false otherwise.
        bool TryGetValue(string meshName, out IModelMesh value)
        {
            ModelMesh modelMesh;
            if (collection.TryGetValue(meshName, out modelMesh))
            {
                value = new ModelMeshAdapter(modelMesh);
                return true;
            }
            value = null;
            return false;
        }
        // Summary:
        //     Determines whether an element is in the System.Collections.ObjectModel.ReadOnlyCollection<T>.
        //
        // Parameters:
        //   value:
        //     The object to locate in the System.Collections.ObjectModel.ReadOnlyCollection<T>.
        //     The value can be null for reference types.
        //
        // Returns:
        //     true if value is found in the System.Collections.ObjectModel.ReadOnlyCollection<T>;
        //     otherwise, false.
        public bool Contains(IModelMesh value) 
        {
            return collection.Contains((value as ModelMeshAdapter).DxModelMesh);
        }
        //
        // Summary:
        //     Copies the entire System.Collections.ObjectModel.ReadOnlyCollection<T> to
        //     a compatible one-dimensional System.Array, starting at the specified index
        //     of the target array.
        //
        // Parameters:
        //   array:
        //     The one-dimensional System.Array that is the destination of the elements
        //     copied from System.Collections.ObjectModel.ReadOnlyCollection<T>. The System.Array
        //     must have zero-based indexing.
        //
        //   index:
        //     The zero-based index in array at which copying begins.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     index is equal to or greater than the length of array.-or-The number of elements
        //     in the source System.Collections.ObjectModel.ReadOnlyCollection<T> is greater
        //     than the available space from index to the end of the destination array.
        //
        //   System.ArgumentOutOfRangeException:
        //     index is less than zero.
        //
        //   System.ArgumentNullException:
        //     array is null.
        public void CopyTo(IModelMesh[] array, int index)
        {
            throw new Exception("Method not implemented.");
        }
        //
        // Summary:
        //     Returns an enumerator that iterates through the System.Collections.ObjectModel.ReadOnlyCollection<T>.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerator<T> for the System.Collections.ObjectModel.ReadOnlyCollection<T>.
        public IEnumerator<IModelMesh> GetEnumerator()
        {
            return new ModelMeshEnumerator(collection.GetEnumerator());
        }
        //
        // Summary:
        //     Searches for the specified object and returns the zero-based index of the
        //     first occurrence within the entire System.Collections.ObjectModel.ReadOnlyCollection<T>.
        //
        // Parameters:
        //   value:
        //     The object to locate in the System.Collections.Generic.List<T>. The value
        //     can be null for reference types.
        //
        // Returns:
        //     The zero-based index of the first occurrence of item within the entire System.Collections.ObjectModel.ReadOnlyCollection<T>,
        //     if found; otherwise, -1.
        public int IndexOf(IModelMesh value)
        {
            return collection.IndexOf((value as ModelMeshAdapter).DxModelMesh);
        }

        private class ModelMeshEnumerator : IEnumerator<IModelMesh>
        {
            private IEnumerator<ModelMesh> enumerator;

            public ModelMeshEnumerator(IEnumerator<ModelMesh> enumerator)
            {
                this.enumerator = enumerator;
            }

            public IModelMesh Current
            {
                get { return new ModelMeshAdapter(enumerator.Current); }
            }

            public void Dispose()
            {
                enumerator.Dispose();
            }

            object System.Collections.IEnumerator.Current
            {
                get { return new ModelMeshAdapter(enumerator.Current); }
            }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                enumerator.Reset();
            }
        }
    }
}
