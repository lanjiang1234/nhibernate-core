using System.Collections;
using System.Data;
using NHibernate.Collection;
using NHibernate.Engine;

namespace NHibernate.Type
{
	/// <summary></summary>
	public class ArrayType : PersistentCollectionType
	{
		//private System.Type elementClass;
		private System.Type arrayClass;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="role"></param>
		/// <param name="elementClass"></param>
		public ArrayType( string role, System.Type elementClass ) : base( role )
		{
			//this.elementClass = elementClass;
			arrayClass = System.Array.CreateInstance( elementClass, 0 ).GetType();
		}

		/// <summary></summary>
		public override System.Type ReturnedClass
		{
			get { return arrayClass; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="persister"></param>
		/// <returns></returns>
		public override PersistentCollection Instantiate( ISessionImplementor session, CollectionPersister persister )
		{
			return new ArrayHolder( session, persister );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="st"></param>
		/// <param name="value"></param>
		/// <param name="index"></param>
		/// <param name="session"></param>
		public override void NullSafeSet( IDbCommand st, object value, int index, ISessionImplementor session )
		{
			base.NullSafeSet( st, session.GetArrayHolder( value ), index, session );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="collection"></param>
		/// <returns></returns>
		public override ICollection GetElementsCollection( object collection )
		{
			return ( ( object[ ] ) collection );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="session"></param>
		/// <returns></returns>
		public override object Disassemble( object value, ISessionImplementor session )
		{
			if( value == null )
			{
				return null;
			}
			return session.GetLoadedCollectionKey( session.GetArrayHolder( value ) );
		}

		/// <summary>
		/// Wraps a <see cref="System.Array"/> in a <see cref="ArrayHolder"/>.
		/// </summary>
		/// <param name="session">The <see cref="ISessionImplementor"/> for the collection to be a part of.</param>
		/// <param name="array">The unwrapped array.</param>
		/// <returns>
		/// An <see cref="ArrayHolder"/> that wraps the non NHibernate <see cref="System.Array"/>.
		/// </returns>
		public override PersistentCollection Wrap( ISessionImplementor session, object array )
		{
			return new ArrayHolder( session, array );
		}

		/// <summary></summary>
		public override bool IsArrayType
		{
			get { return true; }
		}
	}
}