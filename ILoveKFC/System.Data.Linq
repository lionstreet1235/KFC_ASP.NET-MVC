#region Assembly System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.Linq.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Data.Linq
{
    //
    // Summary:
    //     Represents a table for a particular type in the underlying database.
    //
    // Type parameters:
    //   TEntity:
    //     The type of the data in the table.
    public sealed class Table<TEntity> : IQueryable<TEntity>, IEnumerable<TEntity>, IEnumerable, IQueryable, IQueryProvider, ITable, IListSource, ITable<TEntity> where TEntity : class
    {
        private DataContext context;

        private MetaTable metaTable;

        private IBindingList cachedList;

        //
        // Summary:
        //     Gets the System.Data.Linq.DataContext that has been used to retrieve this System.Data.Linq.Table`1.
        //
        // Returns:
        //     The data context that has been used to retrieve this table.
        public DataContext Context => context;

        //
        // Summary:
        //     Gets a value that indicates whether the type of the entities contained in this
        //     System.Data.Linq.Table`1 instance has a primary key.
        //
        // Returns:
        //     true if the entity type does not have a primary key; otherwise, false.
        public bool IsReadOnly => !metaTable.RowType.IsEntity;

        //
        // Summary:
        //     Gets the expression tree that is associated with this instance of System.Linq.IQueryable.
        //
        // Returns:
        //     The System.Linq.Expressions.Expression that is associated with this instance
        //     of System.Linq.IQueryable.
        Expression IQueryable.Expression => Expression.Constant(this);

        //
        // Summary:
        //     Gets the type of the element(s) that are returned when the expression tree associated
        //     with this object is executed.
        //
        // Returns:
        //     The type of the element(s) that are returned when the expression tree associated
        //     with this object is executed.
        Type IQueryable.ElementType => typeof(TEntity);

        //
        // Summary:
        //     Gets the query provider that is associated with this data source.
        //
        // Returns:
        //     The System.Linq.IQueryProvider that is associated with this data source.
        IQueryProvider IQueryable.Provider => this;

        //
        // Summary:
        //     Gets a value that indicates whether the collection is a collection of System.Collections.IList
        //     objects.
        //
        // Returns:
        //     true if the collection is a collection of System.Collections.IList objects; otherwise,
        //     false.
        bool IListSource.ContainsListCollection => false;

        internal Table(DataContext context, MetaTable metaTable)
        {
            this.context = context;
            this.metaTable = metaTable;
        }

        //
        // Summary:
        //     Constructs an System.Linq.IQueryable object that can evaluate the query represented
        //     by a specified expression tree.
        //
        // Parameters:
        //   expression:
        //     The query expression from which the evaluation is performed.
        //
        // Returns:
        //     An System.Linq.IQueryable that can evaluate the query represented by the specified
        //     expression tree.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            if (expression == null)
            {
                throw Error.ArgumentNull("expression");
            }

            Type elementType = TypeSystem.GetElementType(expression.Type);
            Type type = typeof(IQueryable<>).MakeGenericType(elementType);
            if (!type.IsAssignableFrom(expression.Type))
            {
                throw Error.ExpectedQueryableArgument("expression", type);
            }

            Type type2 = typeof(DataQuery<>).MakeGenericType(elementType);
            return (IQueryable)Activator.CreateInstance(type2, context, expression);
        }

        //
        // Summary:
        //     Constructs an System.Linq.IQueryable`1 object that can evaluate the query represented
        //     by a specified expression tree.
        //
        // Parameters:
        //   expression:
        //     The query expression from which the evaluation is performed.
        //
        // Type parameters:
        //   TResult:
        //     The type of the data in the table.
        //
        // Returns:
        //     An System.Linq.IQueryable`1 that can evaluate the query represented by the specified
        //     expression tree.
        IQueryable<TResult> IQueryProvider.CreateQuery<TResult>(Expression expression)
        {
            if (expression == null)
            {
                throw Error.ArgumentNull("expression");
            }

            if (!typeof(IQueryable<TResult>).IsAssignableFrom(expression.Type))
            {
                throw Error.ExpectedQueryableArgument("expression", typeof(IEnumerable<TResult>));
            }

            return new DataQuery<TResult>(context, expression);
        }

        //
        // Summary:
        //     Executes the query represented by a specified expression tree.
        //
        // Parameters:
        //   expression:
        //     The query expression to execute.
        //
        // Returns:
        //     The value that results from executing the specified query.
        object IQueryProvider.Execute(Expression expression)
        {
            return context.Provider.Execute(expression).ReturnValue;
        }

        //
        // Summary:
        //     Executes the strongly-typed query represented by a specified expression tree.
        //
        // Parameters:
        //   expression:
        //     The query expression to execute.
        //
        // Type parameters:
        //   TResult:
        //     The type of the data in the table.
        //
        // Returns:
        //     The value that results from executing the specified query.
        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return (TResult)context.Provider.Execute(expression).ReturnValue;
        }

        //
        // Summary:
        //     Returns an enumerator that iterates through the collection.
        //
        // Returns:
        //     An enumerator that can be used to iterate through the collection.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //
        // Summary:
        //     Returns an enumerator that iterates through the collection.
        //
        // Returns:
        //     An enumerator that can be used to iterate through the collection.
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return GetEnumerator();
        }

        //
        // Summary:
        //     Gets an enumerator that iterates through the collection.
        //
        // Returns:
        //     An enumerator that can be used to iterate through the collection.
        public IEnumerator<TEntity> GetEnumerator()
        {
            return ((IEnumerable<TEntity>)context.Provider.Execute(Expression.Constant(this)).ReturnValue).GetEnumerator();
        }

        //
        // Summary:
        //     Returns an System.Collections.IList that can be bound to a data source from an
        //     object that does not implement an System.Collections.IList itself.
        //
        // Returns:
        //     An System.Collections.IList that can be bound to a data source.
        IList IListSource.GetList()
        {
            if (cachedList == null)
            {
                cachedList = GetNewBindingList();
            }

            return cachedList;
        }

        //
        // Summary:
        //     Creates a new list for binding to a data source.
        //
        // Returns:
        //     A new System.ComponentModel.IBindingList for binding to a data source.
        public IBindingList GetNewBindingList()
        {
            return BindingList.Create(context, this);
        }

        //
        // Summary:
        //     Adds an entity in a pending insert state to this System.Data.Linq.Table`1.
        //
        // Parameters:
        //   entity:
        //     The entity to be added.
        public void InsertOnSubmit(TEntity entity)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            MetaType inheritanceType = metaTable.RowType.GetInheritanceType(entity.GetType());
            if (!IsTrackableType(inheritanceType))
            {
                throw Error.TypeCouldNotBeAdded(inheritanceType.Type);
            }

            TrackedObject trackedObject = context.Services.ChangeTracker.GetTrackedObject(entity);
            if (trackedObject == null)
            {
                trackedObject = context.Services.ChangeTracker.Track(entity);
                trackedObject.ConvertToNew();
            }
            else if (trackedObject.IsWeaklyTracked)
            {
                trackedObject.ConvertToNew();
            }
            else if (trackedObject.IsDeleted)
            {
                trackedObject.ConvertToPossiblyModified();
            }
            else if (trackedObject.IsRemoved)
            {
                trackedObject.ConvertToNew();
            }
            else if (!trackedObject.IsNew)
            {
                throw Error.CantAddAlreadyExistingItem();
            }
        }

        //
        // Summary:
        //     Adds an entity in a pending insert state to this table.
        //
        // Parameters:
        //   entity:
        //     The entity to add.
        void ITable.InsertOnSubmit(object entity)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            TEntity val = entity as TEntity;
            if (val == null)
            {
                throw Error.EntityIsTheWrongType();
            }

            InsertOnSubmit(val);
        }

        //
        // Summary:
        //     Adds all entities of a collection to the System.Data.Linq.DataContext in a pending
        //     insert state.
        //
        // Parameters:
        //   entities:
        //     The entities to add.
        //
        // Type parameters:
        //   TSubEntity:
        //     The type of the elements to insert.
        public void InsertAllOnSubmit<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity
        {
            if (entities == null)
            {
                throw Error.ArgumentNull("entities");
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            List<TSubEntity> list = entities.ToList();
            foreach (TSubEntity item in list)
            {
                TEntity entity = (TEntity)(object)item;
                InsertOnSubmit(entity);
            }
        }

        //
        // Summary:
        //     Adds all entities of a collection to the System.Data.Linq.DataContext in an pending
        //     insert state.
        //
        // Parameters:
        //   entities:
        //     The collection of entities to be added.
        void ITable.InsertAllOnSubmit(IEnumerable entities)
        {
            if (entities == null)
            {
                throw Error.ArgumentNull("entities");
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            List<object> list = entities.Cast<object>().ToList();
            foreach (object item in list)
            {
                ((ITable)this).InsertOnSubmit(item);
            }
        }

        private static bool IsTrackableType(MetaType type)
        {
            if (type == null)
            {
                return false;
            }

            if (!type.CanInstantiate)
            {
                return false;
            }

            if (type.HasInheritance && !type.HasInheritanceCode)
            {
                return false;
            }

            return true;
        }

        //
        // Summary:
        //     Puts an entity from this table into a pending delete state.
        //
        // Parameters:
        //   entity:
        //     The entity to be deleted.
        public void DeleteOnSubmit(TEntity entity)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            TrackedObject trackedObject = context.Services.ChangeTracker.GetTrackedObject(entity);
            if (trackedObject != null)
            {
                if (trackedObject.IsNew)
                {
                    trackedObject.ConvertToRemoved();
                }
                else if (trackedObject.IsPossiblyModified || trackedObject.IsModified)
                {
                    trackedObject.ConvertToDeleted();
                }

                return;
            }

            throw Error.CannotRemoveUnattachedEntity();
        }

        //
        // Summary:
        //     Puts an entity from this table into a pending delete state.
        //
        // Parameters:
        //   entity:
        //     The entity to be removed.
        void ITable.DeleteOnSubmit(object entity)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            TEntity val = entity as TEntity;
            if (val == null)
            {
                throw Error.EntityIsTheWrongType();
            }

            DeleteOnSubmit(val);
        }

        //
        // Summary:
        //     Puts all entities from the collection into a pending delete state.
        //
        // Parameters:
        //   entities:
        //     The entities to delete.
        //
        // Type parameters:
        //   TSubEntity:
        //     The type of the elements to delete.
        public void DeleteAllOnSubmit<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity
        {
            if (entities == null)
            {
                throw Error.ArgumentNull("entities");
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            List<TSubEntity> list = entities.ToList();
            foreach (TSubEntity item in list)
            {
                TEntity entity = (TEntity)(object)item;
                DeleteOnSubmit(entity);
            }
        }

        //
        // Summary:
        //     Puts all entities from the collection into a pending delete state.
        //
        // Parameters:
        //   entities:
        //     The entities being removed.
        void ITable.DeleteAllOnSubmit(IEnumerable entities)
        {
            if (entities == null)
            {
                throw Error.ArgumentNull("entities");
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            List<object> list = entities.Cast<object>().ToList();
            foreach (object item in list)
            {
                ((ITable)this).DeleteOnSubmit(item);
            }
        }

        //
        // Summary:
        //     Attaches a disconnected or "detached" entity to a new System.Data.Linq.DataContext
        //     when original values are required for optimistic concurrency checks.
        //
        // Parameters:
        //   entity:
        //     The original values of the entity to be attached.
        public void Attach(TEntity entity)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            Attach(entity, asModified: false);
        }

        //
        // Summary:
        //     Attaches an entity to the System.Data.Linq.DataContext in an unmodified state.
        //
        // Parameters:
        //   entity:
        //     The entity to be attached.
        void ITable.Attach(object entity)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            TEntity val = entity as TEntity;
            if (val == null)
            {
                throw Error.EntityIsTheWrongType();
            }

            Attach(val, asModified: false);
        }

        //
        // Summary:
        //     Attaches an entity to the System.Data.Linq.DataContext in either a modified or
        //     unmodified state.
        //
        // Parameters:
        //   entity:
        //     The entity to be attached.
        //
        //   asModified:
        //     true to attach the entity as modified; false to attach the entity as unmodified.
        public void Attach(TEntity entity, bool asModified)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            MetaType inheritanceType = metaTable.RowType.GetInheritanceType(entity.GetType());
            if (!IsTrackableType(inheritanceType))
            {
                throw Error.TypeCouldNotBeTracked(inheritanceType.Type);
            }

            if (asModified && inheritanceType.VersionMember == null && inheritanceType.HasUpdateCheck)
            {
                throw Error.CannotAttachAsModifiedWithoutOriginalState();
            }

            TrackedObject trackedObject = Context.Services.ChangeTracker.GetTrackedObject(entity);
            if (trackedObject == null || trackedObject.IsWeaklyTracked)
            {
                if (trackedObject == null)
                {
                    trackedObject = context.Services.ChangeTracker.Track(entity, recurse: true);
                }

                if (asModified)
                {
                    trackedObject.ConvertToModified();
                }
                else
                {
                    trackedObject.ConvertToUnmodified();
                }

                if (Context.Services.InsertLookupCachedObject(inheritanceType, entity) != entity)
                {
                    throw new DuplicateKeyException(entity, Strings.CantAddAlreadyExistingKey);
                }

                trackedObject.InitializeDeferredLoaders();
                return;
            }

            throw Error.CannotAttachAlreadyExistingEntity();
        }

        //
        // Summary:
        //     Attaches all entities of a collection to the System.Data.Linq.DataContext in
        //     either a modified or unmodified state.
        //
        // Parameters:
        //   entity:
        //     The entity to be attached.
        //
        //   asModified:
        //     true to attach the entity as modified; false to attached the entity as unmodified.
        void ITable.Attach(object entity, bool asModified)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            TEntity val = entity as TEntity;
            if (val == null)
            {
                throw Error.EntityIsTheWrongType();
            }

            Attach(val, asModified);
        }

        //
        // Summary:
        //     Attaches an entity to the System.Data.Linq.DataContext in either a modified or
        //     unmodified state by specifying both the entity and its original state.
        //
        // Parameters:
        //   entity:
        //     The entity to be attached.
        //
        //   original:
        //     An instance of the same entity type with data members that contain the original
        //     values.
        public void Attach(TEntity entity, TEntity original)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            if (original == null)
            {
                throw Error.ArgumentNull("original");
            }

            if (entity.GetType() != original.GetType())
            {
                throw Error.OriginalEntityIsWrongType();
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            MetaType inheritanceType = metaTable.RowType.GetInheritanceType(entity.GetType());
            if (!IsTrackableType(inheritanceType))
            {
                throw Error.TypeCouldNotBeTracked(inheritanceType.Type);
            }

            TrackedObject trackedObject = context.Services.ChangeTracker.GetTrackedObject(entity);
            if (trackedObject == null || trackedObject.IsWeaklyTracked)
            {
                if (trackedObject == null)
                {
                    trackedObject = context.Services.ChangeTracker.Track(entity, recurse: true);
                }

                trackedObject.ConvertToPossiblyModified(original);
                if (Context.Services.InsertLookupCachedObject(inheritanceType, entity) != entity)
                {
                    throw new DuplicateKeyException(entity, Strings.CantAddAlreadyExistingKey);
                }

                trackedObject.InitializeDeferredLoaders();
                return;
            }

            throw Error.CannotAttachAlreadyExistingEntity();
        }

        //
        // Summary:
        //     Attaches an entity to the System.Data.Linq.DataContext in either a modified or
        //     unmodified state by specifying both the entity and its original state.
        //
        // Parameters:
        //   entity:
        //     The entity to be attached.
        //
        //   original:
        //     An instance of the same entity type with data members that contain the original
        //     values.
        void ITable.Attach(object entity, object original)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            if (original == null)
            {
                throw Error.ArgumentNull("original");
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            TEntity val = entity as TEntity;
            if (val == null)
            {
                throw Error.EntityIsTheWrongType();
            }

            if (entity.GetType() != original.GetType())
            {
                throw Error.OriginalEntityIsWrongType();
            }

            Attach(val, (TEntity)original);
        }

        //
        // Summary:
        //     Attaches all entities of a collection to the System.Data.Linq.DataContext in
        //     either a modified or unmodified state.
        //
        // Parameters:
        //   entities:
        //     The collection of entities.
        //
        // Type parameters:
        //   TSubEntity:
        //     The type of entities to attach.
        public void AttachAll<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity
        {
            if (entities == null)
            {
                throw Error.ArgumentNull("entities");
            }

            AttachAll(entities, asModified: false);
        }

        //
        // Summary:
        //     Attaches all entities of a collection to the System.Data.Linq.DataContext in
        //     either a modified or unmodified state.
        //
        // Parameters:
        //   entities:
        //     The collection of entities to be attached.
        void ITable.AttachAll(IEnumerable entities)
        {
            if (entities == null)
            {
                throw Error.ArgumentNull("entities");
            }

            ((ITable)this).AttachAll(entities, asModified: false);
        }

        //
        // Summary:
        //     Attaches all entities of a collection to the System.Data.Linq.DataContext in
        //     either a modified or unmodified state.
        //
        // Parameters:
        //   entities:
        //     The collection of entities.
        //
        //   asModified:
        //     true if the object has a timestamp or RowVersion member; false if original values
        //     are being used for the optimistic concurrency check.
        //
        // Type parameters:
        //   TSubEntity:
        //     The type of entities to attach.
        public void AttachAll<TSubEntity>(IEnumerable<TSubEntity> entities, bool asModified) where TSubEntity : TEntity
        {
            if (entities == null)
            {
                throw Error.ArgumentNull("entities");
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            List<TSubEntity> list = entities.ToList();
            foreach (TSubEntity item in list)
            {
                TEntity entity = (TEntity)(object)item;
                Attach(entity, asModified);
            }
        }

        //
        // Summary:
        //     Attaches all entities of a collection to the System.Data.Linq.DataContext in
        //     either a modified or unmodified state.
        //
        // Parameters:
        //   entities:
        //     The collection of entities to be attched.
        //
        //   asModified:
        //     true to attach the entities as modified; false to attach the entities as unmodified.
        void ITable.AttachAll(IEnumerable entities, bool asModified)
        {
            if (entities == null)
            {
                throw Error.ArgumentNull("entities");
            }

            CheckReadOnly();
            context.CheckNotInSubmitChanges();
            context.VerifyTrackingEnabled();
            List<object> list = entities.Cast<object>().ToList();
            foreach (object item in list)
            {
                ((ITable)this).Attach(item, asModified);
            }
        }

        //
        // Summary:
        //     Returns a System.Data.Linq.Table`1 instance that contains the original state
        //     of the entity.
        //
        // Parameters:
        //   entity:
        //     The entity whose original state is to be returned.
        //
        // Returns:
        //     A System.Data.Linq.Table`1 instance that contains the original state of the entity.
        public TEntity GetOriginalEntityState(TEntity entity)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            MetaType metaType = Context.Mapping.GetMetaType(entity.GetType());
            if (metaType == null || !metaType.IsEntity)
            {
                throw Error.EntityIsTheWrongType();
            }

            TrackedObject trackedObject = Context.Services.ChangeTracker.GetTrackedObject(entity);
            if (trackedObject != null)
            {
                if (trackedObject.Original != null)
                {
                    return (TEntity)trackedObject.CreateDataCopy(trackedObject.Original);
                }

                return (TEntity)trackedObject.CreateDataCopy(trackedObject.Current);
            }

            return null;
        }

        //
        // Summary:
        //     Retrieves original values of a given entity.
        //
        // Parameters:
        //   entity:
        //     The entity whose original state is to be accessed.
        //
        // Returns:
        //     A copy of the original entity.
        object ITable.GetOriginalEntityState(object entity)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            TEntity val = entity as TEntity;
            if (val == null)
            {
                throw Error.EntityIsTheWrongType();
            }

            return GetOriginalEntityState(val);
        }

        //
        // Summary:
        //     Returns an array of modified members that contain their current and original
        //     values.
        //
        // Parameters:
        //   entity:
        //     The entity from which to get the array.
        //
        // Returns:
        //     An array of modified members that contain their current and original values.
        public ModifiedMemberInfo[] GetModifiedMembers(TEntity entity)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            MetaType metaType = Context.Mapping.GetMetaType(entity.GetType());
            if (metaType == null || !metaType.IsEntity)
            {
                throw Error.EntityIsTheWrongType();
            }

            TrackedObject trackedObject = Context.Services.ChangeTracker.GetTrackedObject(entity);
            if (trackedObject != null)
            {
                return trackedObject.GetModifiedMembers().ToArray();
            }

            return new ModifiedMemberInfo[0];
        }

        //
        // Summary:
        //     Returns an array of modified members that contain their current and original
        //     values.
        //
        // Parameters:
        //   entity:
        //     The entity from which to obtain the modified members.
        //
        // Returns:
        //     An array of modified members.
        ModifiedMemberInfo[] ITable.GetModifiedMembers(object entity)
        {
            if (entity == null)
            {
                throw Error.ArgumentNull("entity");
            }

            TEntity val = entity as TEntity;
            if (val == null)
            {
                throw Error.EntityIsTheWrongType();
            }

            return GetModifiedMembers(val);
        }

        private void CheckReadOnly()
        {
            if (IsReadOnly)
            {
                throw Error.CannotPerformCUDOnReadOnlyTable(ToString());
            }
        }

        //
        // Summary:
        //     Returns a string that represents the table.
        //
        // Returns:
        //     A string representation of the table.
        public override string ToString()
        {
            return "Table(" + typeof(TEntity).Name + ")";
        }
    }
}
#if false // Decompilation log
'36' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll'
------------------
Resolve: 'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.dll'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll'
------------------
Resolve: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.dll'
------------------
Resolve: 'System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.Transactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.Transactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.dll'
------------------
Resolve: 'System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.Linq.dll'
#endif
