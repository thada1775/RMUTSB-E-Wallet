package crc6422f184d23f1e08ee;


public class PinItemViewRenderer
	extends crc64720bb2db43a66fe9.ViewRenderer_2
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("FormsPinView.Droid.PinItemViewRenderer, FormsPinView.Droid", PinItemViewRenderer.class, __md_methods);
	}


	public PinItemViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == PinItemViewRenderer.class)
			mono.android.TypeManager.Activate ("FormsPinView.Droid.PinItemViewRenderer, FormsPinView.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public PinItemViewRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == PinItemViewRenderer.class)
			mono.android.TypeManager.Activate ("FormsPinView.Droid.PinItemViewRenderer, FormsPinView.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public PinItemViewRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == PinItemViewRenderer.class)
			mono.android.TypeManager.Activate ("FormsPinView.Droid.PinItemViewRenderer, FormsPinView.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
