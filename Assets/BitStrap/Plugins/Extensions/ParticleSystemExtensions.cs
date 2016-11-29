using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Extensions to the ParticleSystem class.
	/// </summary>
	public static class ParticleSystemExtensions
	{
		/// <summary>
		/// Enables/disables the emission module in a ParticleSystem.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="enabled"></param>
		public static void EnableEmission( this ParticleSystem self, bool enabled )
		{
			ParticleSystem.EmissionModule emission = self.emission;
			emission.enabled = enabled;
		}

		/// <summary>
		/// Workaround for the deprecated ParticleSystem.emissionRate getter.
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static float GetEmissionRate( this ParticleSystem self )
		{
			return self.emission.rate.constantMax;
		}

		/// <summary>
		/// Workaround for the deprecated ParticleSystem.emissionRate setter.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="emissionRate"></param>
		public static void SetEmissionRate( this ParticleSystem self, float emissionRate )
		{
			ParticleSystem.EmissionModule emission = self.emission;
			ParticleSystem.MinMaxCurve rate = emission.rate;
			rate.constantMax = emissionRate;
			emission.rate = rate;
		}
	}
}
